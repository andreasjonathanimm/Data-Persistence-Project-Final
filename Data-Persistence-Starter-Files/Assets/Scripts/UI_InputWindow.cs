using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InputWindow : MonoBehaviour
{
    private static UI_InputWindow instance;
    private InputField inputField;
    private MainManager mainManager;
    private void Awake()
    {
        instance = this;
        inputField = transform.Find("Input Field").GetComponent<InputField>();
        mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
        Hide();
    }

    private void Update()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
        {
            OnComplete();
        }
    }

    private void Show(string validCharacters)
    {
        gameObject.SetActive(true);
        inputField.onValidateInput = (string text, int charIndex, char addedChar) =>
        {
            return ValidateChar(validCharacters, addedChar);
        };
        inputField.ActivateInputField();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private char ValidateChar(string validCharacters, char addedChar)
    {
        if (validCharacters.IndexOf(addedChar) != -1)
        {
            return addedChar;
        } else
        {
            return '\0';
        }
    }

    public static void Show_Static(string validCharacters)
    {
        instance.Show(validCharacters);
    }

    public void OnComplete()
    {
        mainManager.newName = inputField.text;
        Hide();
    }
}
