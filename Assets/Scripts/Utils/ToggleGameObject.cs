using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Disables/enables a given game object
/// </summary>

public class ToggleGameObject : MonoBehaviour {

    [SerializeField]
    GameObject target;

    public void ToggleActiveState()
    {
        bool currentState = target.activeInHierarchy;
        target.SetActive(!currentState);
    }
}

