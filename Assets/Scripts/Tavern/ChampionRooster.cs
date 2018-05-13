using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChampionRooster : MonoBehaviour {

    [SerializeField]
    GameObject championRooster;
    GameObject championSelection;


    [Header("New Champion Creation")]
    [SerializeField]
    Button createChampion;
    [SerializeField]
    GameObject championCreationScreen;

    void Start ()
    {
        InitializeVariables();
        AddButtonListeners();
	}
	

    void AddButtonListeners()
    {
        createChampion.onClick.AddListener(OpenChampionCreation);
    }

    void InitializeVariables()
    {
    }

    void OpenChampionCreation()
    {
        championCreationScreen.SetActive(true);
        championRooster.SetActive(false);
    }
}
