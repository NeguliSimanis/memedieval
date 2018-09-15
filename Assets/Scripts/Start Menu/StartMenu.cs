using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Upon opening the game, check if save file exists.
/// If yes, load it directly and go to map scene.
/// 
/// 15.09.2018.
/// </summary>

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    bool loadGameAutomatically = true;
    string levelToLoad = "Test scene";

	void Start ()
    {
        if (!loadGameAutomatically)
            return;

        SaveLoad saveLoad = gameObject.GetComponent<SaveLoad>();

        // check if save file exists
        if (saveLoad.CheckSaveFile())
        {
            // save exists - load data
            saveLoad.Load();

            // load scene
            gameObject.GetComponent<LoadScene>().loadLevel(levelToLoad);
        }
    }
	

}
