using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseWhileOptionsOpen : MonoBehaviour {

    bool isPaused = false;

    public void Pause()
    {
        isPaused = !isPaused;

        if (isPaused)
            Time.timeScale = 0f;
        else
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
    }

}
