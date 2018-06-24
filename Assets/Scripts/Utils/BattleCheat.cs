using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// script for testing batle
/// press "w" to win battle
/// </summary>

public class BattleCheat : MonoBehaviour
{
    string playerCastleTag = "Player castle";
    bool isCheatActive = false;

    void Update ()
    {
	    if (Input.GetKey(KeyCode.W))
        {
            if (!isCheatActive)
            {
                isCheatActive = true;
                Debug.Log("Victory cheat");
                GameObject.FindGameObjectWithTag(playerCastleTag).GetComponent<Health>().WinBattle();
            }          
        }
    }
}
