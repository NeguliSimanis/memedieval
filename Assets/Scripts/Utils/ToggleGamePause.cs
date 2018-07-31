using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGamePause : MonoBehaviour {


	public void UnPause()
    {
        PlayerProfile.Singleton.gameObject.transform.Find("TimeControl").GetComponent<TimeControl>().Unpause();
    }
}
