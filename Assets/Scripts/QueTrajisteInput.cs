using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueTrajisteInput : MonoBehaviour
{
    TMPro.TMP_InputField inputField;
    string lastText;
    string text;
    float timer;
    public string[] words;

    void Start()
    {
        inputField = transform.Find("Text").GetComponent<TMPro.TMP_InputField>();
        inputField.caretWidth = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // If input starts with 'w', erase everything
        if (!string.IsNullOrEmpty(inputField.text) && inputField.text.StartsWith("w"))
        {
            inputField.text = "";
        }

        if (text == lastText) timer += Time.deltaTime;
        else timer = 0f;

        inputField.ActivateInputField();
        text = inputField.text.ToLower();

        lastText = text;

        if (timer > 0f)
        {
            foreach (string item in words)
            {
                float sim = Subtitles.Similarity(text, item);

                if (sim > 0.9f)
                {
                    GameObject.Find("GameManager").GetComponent<GameManager>().DoWhatGoesNext();
                }
            }
        }
    }
}
