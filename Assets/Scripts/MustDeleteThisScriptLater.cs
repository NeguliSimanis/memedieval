using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MustDeleteThisScriptLater : MonoBehaviour {

    /// <summary>
    ///  Added this script so that I could pause it during presentation
    /// </summary>

    bool isPaused = false;

	// Update is called once per frame
	void Update () { 
        if (Input.GetKey(KeyCode.P))
        {
            if (isPaused == false)
            {
                isPaused = true;
                Time.timeScale = 0;
            }
            else
            {
                isPaused = false;
                Time.timeScale = 1;
            }
        }
            

    }
}
