using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeutralChampions : MonoBehaviour {

    CreateChampion createChampion;
    string createChampionName = "ChampionCreate";
    int startingChampionsCount = 3; // with how many neutral champions the game starts

    public List<Champion> neutralChampionsList = new List<Champion>();

    void Awake ()
    {
        InitializeVariables();
        GenerateRandomChampion(startingChampionsCount);
	}

    void InitializeVariables()
    {
        createChampion = transform.parent.Find(createChampionName).gameObject.GetComponent<CreateChampion>();
    }
	
    public void GenerateRandomChampion(int championCount)
    {
        for (int i = 0; i < championCount; i++)
        {
            neutralChampionsList.Add(createChampion.CreateRandomChamp(this.gameObject));
        }
    }
    
}
