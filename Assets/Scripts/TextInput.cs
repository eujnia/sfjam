using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextInput : MonoBehaviour
{
    public int id = 0;
    public bool active;
    TMPro.TMP_InputField inputField;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            TextInput[] allInputs = FindObjectsOfType<TextInput>();
            foreach (TextInput input in allInputs)
            {
                input.active = false;
            }
            active = true;
        });
        inputField = transform.Find("Text").GetComponent<TMPro.TMP_InputField>();
        inputField.caretWidth = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            inputField.ActivateInputField();
            if(id == 0) Config.Instance.data.myName = inputField.text;
            else if(id == 1) Config.Instance.data.otherName = inputField.text;
        }
    }
}
