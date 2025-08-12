using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class TextInputVisual : MonoBehaviour
{
    [SerializeField] private TMP_InputField textInput;
    [SerializeField] private Button enterBtn;
    public event EventHandler<string> OnTextEnter;
    
    void Start()
    {
        // right now I'm initializing in the start method
        enterBtn.onClick.AddListener(OnEnterBtnClick);
        
    }

    private void OnEnterBtnClick() 
    {
        // get the text
        string text = textInput.text;

        // for debugging remove this later 
        Debug.Log($"the text entered -> {text}");

        // clear the text input
        textInput.text = "";
        OnTextEnter?.Invoke(this, text);

    }

    private void OnDestroy()
    {
        enterBtn.onClick.RemoveListener(OnEnterBtnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
