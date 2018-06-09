using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseWhileOptionsOpen : MonoBehaviour {

	void Update ()
    {
		if (gameObject.activeSelf == true)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
	}
}
