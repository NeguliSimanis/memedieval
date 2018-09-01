﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleOver
{
    /// <summary>
    /// Deals with consequences of winning or losing battles
    /// TO-DO: move functionality from Health script to here
    /// </summary>

    public static BattleOver manager;

    #region tags
    string enemyBalancerTag = "Enemy balancer";
    string victoryPopupTag = "Victory popup";
    string defeatPopupTag = "Defeat popup";
    string saltAmountTag = "Victory salt";
    string ducatAmountTag = "Victory ducat";
    #endregion

    #region battle spoils
    int saltPerWin = 5;
    int currentSaltGain = 0; // how much salt gained in current battle
    int currentDucatGain = 0; // how many ducats gained in current battle
    #endregion

    #region new champion generation
    int maxNeutralChampionCount = 3;
    int newChampionsPerBattle = 1;
    float newChampionChance = 0.5f; // the chance of a new champion appearing after each battle
    #endregion

    public void EndBattle(bool isVictory = false)
    {
        ResetValues();
        GenerateNewChampions();
        if (isVictory)
        {
            ObtainVictorySpoils();
            WinBattle();
        }
        else
        {
            LoseBattle();
        }
    }

    private void ResetValues()
    {
        currentSaltGain = 0;
        currentDucatGain = 0;
    }

    public void WinBattle()
    {
        DisplayVictoryPopup();
        AllocateExp();

        if (GameData.current == null)
        {
            GameData.current = new GameData();
        }

        // mark castle as destroyed
        int defeatedCastleID = GameObject.FindGameObjectWithTag(enemyBalancerTag).GetComponent<EnemyBalancer>().currentCastleID;
        GameData.current.destroyedCastles[defeatedCastleID] = true;
        GameData.current.lastDestroyedCastle = defeatedCastleID;

        // reset effect from champion skills
        PlayerProfile.Singleton.gameObject.GetComponent<ChampionEffect>().ResetChampionEffect();  
    }

    private void LoseBattle()
    {
        DisplayDefeatPopup();
    }

    // allocates exp to champions in case of victory in battle
    private void AllocateExp()
    {
        ExpController expController = GameObject.FindGameObjectWithTag("BattleController").GetComponent<ExpController>();
        expController.AllocateChampionExp();
    }

    private void DisplayVictoryPopup()
    {
        // enables popup game object
        ToggleGameObject popupEnabler = GameObject.FindGameObjectWithTag(victoryPopupTag).GetComponent<ToggleGameObject>();
        popupEnabler.ToggleActiveState();

        // update text with victory spoils 
        GameObject.FindGameObjectWithTag(saltAmountTag).GetComponent<Text>().text = currentSaltGain.ToString();
        GameObject.FindGameObjectWithTag(ducatAmountTag).GetComponent<Text>().text = currentDucatGain.ToString();
    }

    private void DisplayDefeatPopup()
    {
        // enables popup game object
        ToggleGameObject popupEnabler = GameObject.FindGameObjectWithTag(defeatPopupTag).GetComponent<ToggleGameObject>();
        popupEnabler.ToggleActiveState();
    }

    // random chance to make new neutral champions after battle 
    private void GenerateNewChampions()
    {
        float newChampionRoll = Random.Range(0f, 1f);

        // did not roll a new champion, end function
        if (newChampionRoll > newChampionChance)
            return;

        // rolled a new champion, find necessary component
        NeutralChampions neutralChampions = PlayerProfile.Singleton.gameObject.transform.Find("ChampionRooster").gameObject.GetComponent<NeutralChampions>();

        // creates new neutral champion
        if (neutralChampions.neutralChampionsList.Count < maxNeutralChampionCount)
        {
            Debug.Log("new champion created");
            neutralChampions.GenerateRandomChampion(newChampionsPerBattle);
        }
    }


    void ObtainVictorySpoils()
    {
        PlayerProfile playerProfile = PlayerProfile.Singleton;

        //salt
        playerProfile.SaltCurrent += saltPerWin;
        currentSaltGain = saltPerWin;

        //ducats
        float obtainDucatChance = playerProfile.gameObject.GetComponent<ChampionEffect>().ducatFindChance;
        float ducatRoll = Random.Range(0f, 0.99f);

        if (obtainDucatChance > ducatRoll)
        {
            playerProfile.DucatCurrent++;
            currentDucatGain = 1;
        }
    }
}
