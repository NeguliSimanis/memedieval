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
        // INSTANT BATTLE VICTORY
	    if (Input.GetKey(KeyCode.W))
        {
            if (!isVictoryCheatActive)
            {
                isVictoryCheatActive = true;
                GameObject.FindGameObjectWithTag(playerCastleTag).GetComponent<Health>().EndBattle(true);
            }          
        }
        // INSTANT MEAT IN BATTLE
        if (Input.GetKeyDown(KeyCode.M))
        {
            playerResources.Amount += meatPerCheat;
        }
        // UNLOCK ALL MECHANICS
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnlockAllMechanics();
        }
        // RELOAD CURRENT LEVEL
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadLevel();
        }
        // GET MORE SALT
        if (Input.GetKeyDown(KeyCode.S))
        {
            GetSaltCheat();
        }
    }

    void GetSaltCheat()
    {
        PlayerProfile.Singleton.SaltCurrent += 10;
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
