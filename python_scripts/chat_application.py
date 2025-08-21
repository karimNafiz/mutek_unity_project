#!/usr/bin/env python3
import json
import uuid
from http.server import BaseHTTPRequestHandler, HTTPServer
from urllib.parse import urlparse
import ollama
import os
import time








# ===== Global State =====
CURRENT_SESSION_ID = None          # str | None
CHAT_HISTORY = []                  # list[str] (user messages only)

# --- File writer globals ---
LOG_FILE_HANDLE = None             # file object | None
LOG_FILE_OPEN = False              # bool
LOG_FILE_PATH = None               # str | None

# ===== LLM Call =====
def call_surveillance_llm(new_message: str, history: list[str]) -> dict:
    """
    Calls the surveillance model with {chat_history, new_message}.
    Expects JSON back with keys: suspicion_score (int/float) and reasoning (str).
    """
    payload = {
        "chat_history": history,
        "new_message": new_message,
    }
    print("payload ", payload)

    try:
        resp = ollama.chat(
            model="v2_1984_sur:latest",  # <-- change if needed // could be a CLI arg/env
            messages=[{"role": "user", "content": json.dumps(payload)}],
            format="json",
        )
        content = resp["message"]["content"]
        data = json.loads(content)

        return {
            "suspicion_score": data.get("suspicion_score", data.get("score", 0)),
            "reasoning": data.get("reasoning", "No reasoning provided."),
        }
    except Exception as e:
        return {
            "suspicion_score": 0,
            "reasoning": f"Surveillance model unavailable or invalid response: {e}",
        }

# ===== File writer helpers =====
def _close_log_if_open():
    """Close the current log file if it's open."""
    global LOG_FILE_HANDLE, LOG_FILE_OPEN, LOG_FILE_PATH
    if LOG_FILE_OPEN and LOG_FILE_HANDLE is not None:
        try:
            LOG_FILE_HANDLE.flush()
            LOG_FILE_HANDLE.close()
        except Exception:
            pass
    LOG_FILE_HANDLE = None
    LOG_FILE_OPEN = False
    LOG_FILE_PATH = None

def _open_new_log_for_session(session_id: str):
    """
    Open a new log file for the given session.
    Creates a 'logs' directory beside this script if it doesn't exist.
    """
    global LOG_FILE_HANDLE, LOG_FILE_OPEN, LOG_FILE_PATH

    logs_dir = os.path.join(os.getcwd(), "logs")
    os.makedirs(logs_dir, exist_ok=True)

    # filename includes timestamp + session id for uniqueness & traceability
    ts = time.strftime("%Y%m%d-%H%M%S", time.localtime())
    filename = f"session_{ts}_{session_id}.jsonl"
    path = os.path.join(logs_dir, filename)

    LOG_FILE_HANDLE = open(path, "a", encoding="utf-8")
    LOG_FILE_OPEN = True
    LOG_FILE_PATH = path

    # Optional header line to mark session start (comment line for readability)
    LOG_FILE_HANDLE.write(f'# New session {session_id} started at {ts}\n')
    LOG_FILE_HANDLE.flush()

def _append_message_record(history, user_message, suspicion_score, reasoning):
    """
    Append one JSON object to the session log with the required shape.
    Uses JSON Lines format (one object per line) for easy streaming/tailing.
    """
    global LOG_FILE_HANDLE, LOG_FILE_OPEN
    if not LOG_FILE_OPEN or LOG_FILE_HANDLE is None:
        return  # silently skip if no log is open

    record = {
        "chat_history": history,           # list of user messages so far
        "user_message": user_message,      # the just-received user message
        "suspicion_score": suspicion_score,
        "reasoning": reasoning,
    }
    try:
        LOG_FILE_HANDLE.write(json.dumps(record, ensure_ascii=False) + "\n")
        LOG_FILE_HANDLE.flush()
    except Exception as e:
        # If logging fails, don't break the request flow
        print(f"[log write error] {e}")

# ===== HTTP Server =====
class SimpleHandler(BaseHTTPRequestHandler):
    server_version = "SimpleSurveillance/0.2"

    # Helpers
    def _send_json(self, status: int, body: dict):
        data = json.dumps(body).encode("utf-8")
        self.send_response(status)
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

        # --- Create new session & rotate log ---
        if path == "/session/new":
            # Close any existing session log
            _close_log_if_open()

            # Create a new session and clear the chat history
            CURRENT_SESSION_ID = str(uuid.uuid4())
            CHAT_HISTORY = []

            # Open a fresh log file for this session
            _open_new_log_for_session(CURRENT_SESSION_ID)

            return self._send_json(200, {
                "session_id": CURRENT_SESSION_ID,
                "message": "New session created. Chat history cleared.",
                "log_file": LOG_FILE_PATH,
            })

        # --- Receive message, call LLM, append to log ---
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

            # Write to log BEFORE mutating history, so the record's chat_history
            # reflects exactly what the model saw (pre-append).
            _append_message_record(
                history=CHAT_HISTORY,
                user_message=user_message,
                suspicion_score=result.get("suspicion_score", 0),
                reasoning=result.get("reasoning", "No reasoning."),
            )

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
        try:
            httpd.serve_forever()
        finally:
            # Ensure the log is closed if the server is stopped
            _close_log_if_open()

if __name__ == "__main__":
    run()
