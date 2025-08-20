#!/usr/bin/env python3
import json
import uuid
from http.server import BaseHTTPRequestHandler, HTTPServer
from urllib.parse import urlparse
import ollama

# ===== Global State =====
CURRENT_SESSION_ID = None          # str | None
CHAT_HISTORY = []                  # list[str] (user messages only)

# ===== LLM Call =====
def call_surveillance_llm(new_message: str, history: list[str]) -> dict:
    """
    Calls the surveillance model with {chat_history, new_message}.
    Expects JSON back with keys: suspicion_score (int/float) and reasoning (str).
    """
    # create payload for the surveillance LLM
    payload = {
        "chat_history": history,
        "new_message": new_message,
    }
    print("payload ",payload)


    # try sending it to the backend 
    # if we encounter an exception
    # we send a suspicion score of 0, and some bullshit reasoning
    try:
        resp = ollama.chat(
            model="v2_1984_sur:latest",            # <-- change if needed // need to take as command line argument
            messages=[{"role": "user", "content": json.dumps(payload)}],
            format="json",
        )
        # Ollama responses are typically dicts with resp["message"]["content"]
        content = resp["message"]["content"]
        data = json.loads(content)

        # Normalize output with sensible defaults
        return {
            "suspicion_score": data.get("suspicion_score", data.get("score", 0)),
            "reasoning": data.get("reasoning", "No reasoning provided."),
        }
    except Exception as e:
        # Fallback response if the model errors or returns bad JSON
        return {
            "suspicion_score": 0,
            "reasoning": f"Surveillance model unavailable or invalid response: {e}",
        }

# ===== HTTP Server =====
class SimpleHandler(BaseHTTPRequestHandler):
    server_version = "SimpleSurveillance/0.1"

    # Helpers
    def _send_json(self, status: int, body: dict):
        data = json.dumps(body).encode("utf-8")
        self.send_response(status) # tf are we sending response before header
        self.send_header("Content-Type", "application/json; charset=utf-8")
        self.send_header("Content-Length", str(len(data)))
        # Optional: very permissive CORS for local dev / Unity editor
        self.send_header("Access-Control-Allow-Origin", "*")
        self.send_header("Access-Control-Allow-Headers", "Content-Type")
        self.end_headers()
        self.wfile.write(data)

    def _read_json(self) -> dict:
        try:
            length = int(self.headers.get("Content-Length", "0"))
            raw = self.rfile.read(length) if length > 0 else b"{}"
            return json.loads(raw.decode("utf-8") or "{}")
        except Exception:
            return {}

    def do_OPTIONS(self):
        # CORS preflight (if you call from Unity WebGL or a browser)
        self.send_response(204)
        self.send_header("Access-Control-Allow-Origin", "*")
        self.send_header("Access-Control-Allow-Methods", "POST, OPTIONS")
        self.send_header("Access-Control-Allow-Headers", "Content-Type")
        self.end_headers()

    def do_POST(self):
        global CURRENT_SESSION_ID, CHAT_HISTORY

        path = urlparse(self.path).path

        if path == "/session/new":
            # Create a new session and clear the chat history
            CURRENT_SESSION_ID = str(uuid.uuid4())
            CHAT_HISTORY = []
            return self._send_json(200, {
                "session_id": CURRENT_SESSION_ID,
                "message": "New session created. Chat history cleared."
            })

        elif path == "/message":
            body = self._read_json()
            session_id = body.get("session_id")
            user_message = body.get("message")

            if not session_id or not isinstance(user_message, str):
                return self._send_json(400, {
                    "error": "Missing or invalid 'session_id' or 'message'."
                })

            if session_id != CURRENT_SESSION_ID or CURRENT_SESSION_ID is None:
                return self._send_json(409, {
                    "error": "Invalid or expired session_id.",
                    "hint": "Call /session/new to start a session."
                })

            # Use prior chat history + new message
            result = call_surveillance_llm(user_message, CHAT_HISTORY)

            # Append the user message to history (user-only, as requested)
            CHAT_HISTORY.append(user_message)

            return self._send_json(200, {
                "session_id": CURRENT_SESSION_ID,
                "reasoning": result.get("reasoning", "No reasoning."),
                "suspicion_score": result.get("suspicion_score", 0),
                "history_length": len(CHAT_HISTORY),
            })

        else:
            return self._send_json(404, {"error": "Route not found."})

def run(host="127.0.0.1", port=8080):
    with HTTPServer((host, port), SimpleHandler) as httpd:
        print(f"Listening on http://{host}:{port}")
        httpd.serve_forever()

if __name__ == "__main__":
    run()
