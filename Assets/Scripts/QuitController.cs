using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// opens confirmation window when ESCAPE pressed
/// exits game when ESCAPE pressed twice or action is confirmed in the window
/// </summary>

public class QuitController : MonoBehaviour
{
    [SerializeField]
    GameObject quitGamePopop;

    TimeControl timeController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (quitGamePopop.activeInHierarchy == false)
            {
                PauseGame();
                quitGamePopop.SetActive(true); 
            }
            else
            {
                Debug.Log("QUITTING BRO");
                Application.Quit();
            }  
        }
    }

    void FindTimeControl()
    {
        GameObject player = PlayerProfile.Singleton.gameObject;
        timeController = player.transform.Find("TimeControl").gameObject.GetComponent<TimeControl>();     
    }

    void PauseGame()
    {
        if (timeController == null)
        {    
            FindTimeControl();
            if (timeController != null)
            {
                timeController.Pause();
            }
        }
        else
        {
            timeController.Pause();
        }
    }

    public void Unpause()
    {
        if (timeController == null)
        {
            FindTimeControl();
            if (timeController != null)
            {
                timeController.Unpause();
            }
        }
        else
        {
            timeController.Unpause();
        }

    }
}