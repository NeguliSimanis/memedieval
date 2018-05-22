using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// class used in Tavern stats page
public class ChampionButton : MonoBehaviour
{
    public Champion champion;
    public GameObject buttonObject;

    public ChampionButton(Champion newChampion, GameObject newButtonObject)
    {
        champion = newChampion;
        buttonObject = newButtonObject;
    }
}
