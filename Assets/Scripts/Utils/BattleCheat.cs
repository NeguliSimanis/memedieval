using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// script for testing batle
/// press "w" to win battle
/// </summary>

public class BattleCheat : MonoBehaviour
{
    string playerCastleTag = "Player castle";
    string levelToLoad = "Test scene";

    bool isVictoryCheatActive = false;
    bool areMechanicsUnlocked = false;

    int meatPerCheat = 10;
    [SerializeField] private MeMedieval.Resources playerResources; 

    void Update ()
    {
	    if (Input.GetKey(KeyCode.W))
        {
            if (!isVictoryCheatActive)
            {
                isVictoryCheatActive = true;
                GameObject.FindGameObjectWithTag(playerCastleTag).GetComponent<Health>().EndBattle(true);
            }          
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            playerResources.Amount += meatPerCheat;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            UnlockAllMechanics();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadLevel();
        }
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    /// <summary>
    /// 07.09.2018
    /// </summary>
    void UnlockAllMechanics()
    {
        if (areMechanicsUnlocked)
            return;
        areMechanicsUnlocked = false;
        if(UnlockMechanics.current == null)
        {
            UnlockMechanics.current = new UnlockMechanics();
        }
        UnlockMechanics.current.UnlockAll();
        ReloadLevel();
    }
}
