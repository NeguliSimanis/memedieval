using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleOver
{
    /// <summary>
    /// Deals with consequences of winning or losing battles
    /// TO-DO: move functionality from Health script to here
    /// </summary>

    public static BattleOver manager;

    #region new champion generation
    int maxNeutralChampionCount = 3;
    int newChampionsPerBattle = 1;
    float newChampionChance = 0.5f; // the chance of a new champion appearing after each battle
    #endregion

    public void EndBattle()
    {
        GenerateNewChampions();
    }

    // makes new neutral champions after battle
    void GenerateNewChampions()
    {
        float newChampionRoll = Random.Range(0f, 1f);

        if (newChampionRoll > newChampionChance)
            return;

        NeutralChampions neutralChampions = PlayerProfile.Singleton.gameObject.transform.Find("ChampionRooster").gameObject.GetComponent<NeutralChampions>();

        if (neutralChampions.neutralChampionsList.Count < maxNeutralChampionCount)
        {
            Debug.Log("new champion created");
            neutralChampions.GenerateRandomChampion(newChampionsPerBattle);
        }
    }
}
