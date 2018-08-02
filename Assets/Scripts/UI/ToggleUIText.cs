using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUIText : MonoBehaviour
{
    /// <summary>
    /// Toggles between a given list of strings to change a UI text
    /// 
    /// 01.08.2018 Sīmanis Mikoss
    /// </summary>

    [SerializeField] string[] texts;
    [SerializeField] Text textToToggle;
    private int currentTextID = 0;

    public void ToggleText()
    {
        currentTextID++;
        if (currentTextID >= texts.Length)
            currentTextID = 0;

        textToToggle.text = texts[currentTextID];
    }
}
