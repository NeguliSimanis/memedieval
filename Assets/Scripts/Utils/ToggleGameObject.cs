using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Disables/enables a given game object
/// </summary>

public class ToggleGameObject : MonoBehaviour {

    [SerializeField]
    GameObject[] targets;

    public void ToggleActiveState()
    {
        foreach (GameObject target in targets)
        {
            bool currentState = target.activeInHierarchy;
            target.SetActive(!currentState);
        }
    }
}

