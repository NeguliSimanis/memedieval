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
    bool isVictoryCheatActive = false;

    int meatPerCheat = 10;
    [SerializeField] private MeMedieval.Resources playerResources; 

    void Update ()
    {
	    if (Input.GetKey(KeyCode.W))
        {
            if (!isVictoryCheatActive)
            {
                isVictoryCheatActive = true;
                GameObject.FindGameObjectWithTag(playerCastleTag).GetComponent<Health>().WinBattle();
            }          
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            playerResources.Amount += meatPerCheat;
        }
    }
}
