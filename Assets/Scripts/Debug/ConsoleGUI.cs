using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleGUI : MonoBehaviour
{
    string log;

    private void OnEnable()
    {
        Application.logMessageReceived += OnConsoleMessage;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= OnConsoleMessage;
    }

    private void OnConsoleMessage(string condition, string stackTrace, LogType type)
    {
        log = condition + "\n" + log;
    }

    private void OnGUI()
    {
        Rect textRect = new Rect(10, 10, Screen.width - 10, Screen.height - 10);
        log = GUI.TextArea(textRect, log);
    }
}
