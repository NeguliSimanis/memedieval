using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControl : MonoBehaviour {

    /// <summary>
    /// manages unit movement etc. when game is paused or the game speed is changed
    /// </summary>

    public static bool isGamePaused = false;

    public void Pause()
    {
        if (isGamePaused)
        {
            return;
        }
        isGamePaused = true;
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        if (!isGamePaused)
        {
            return;
        }
        isGamePaused = false;
        Time.timeScale = 1f;
    }
}
