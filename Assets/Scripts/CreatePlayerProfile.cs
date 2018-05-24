using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlayerProfile : MonoBehaviour {

    [SerializeField]
    GameObject playerProfilePrefab;
    [SerializeField]
    string playerProfileTag = "Player profile";

	void Awake()
    {
        if (GameObject.FindGameObjectWithTag(playerProfileTag) == null)
            Instantiate(playerProfilePrefab);
    }
}
