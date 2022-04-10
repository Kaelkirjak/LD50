using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaseCounter : MonoBehaviour
{
    [Header("Counter variables")]
    [Tooltip("Text component")][SerializeField]                         TMP_Text text;
    [Tooltip("Text start")][SerializeField]                             string startText;
    [Tooltip("End text")][SerializeField]                               string endText;

    /// <summary>
    /// Change the displayed text
    /// </summary>
    /// <param name="newText">New text to display</param>
    protected void ChangeText(string newText)
    {
        text.text = $"{startText}{newText}{endText}";
    }
}
