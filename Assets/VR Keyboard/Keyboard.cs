using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour
{    
    public TMPro.TMP_InputField inputField;

    // Update is called once per frame

    public void PressKey(string key)
    {
        Debug.Log("Pressed key: " + key);
        UpdateInputField(key);
    }

    public void ObliterateChar()
    {
        if (inputField.text.Length > 0)
        {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
        }
    }
    private void UpdateInputField(string key)
    {
        inputField.text += key;
    }

}
