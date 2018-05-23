using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeutralChampions : MonoBehaviour {

    CreateChampion createChampion;
    string createChampionName = "ChampionCreate";

    public List<Champion> neutralChampionsList = new List<Champion>();

    void Awake ()
    {
        InitializeVariables();
        GenerateRandomChampion(2);
	}

    void InitializeVariables()
    {
        createChampion = transform.parent.Find(createChampionName).gameObject.GetComponent<CreateChampion>();
    }
	
    void GenerateRandomChampion(int championCount)
    {
        for (int i = 0; i < championCount; i++)
        {
            neutralChampionsList.Add(createChampion.createRandomChamp(this.gameObject));
        }
    }
    
}
