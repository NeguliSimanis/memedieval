using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // to get the name of the selected button

public class Map : MonoBehaviour {

    #region variables
    [SerializeField]
    GameObject battleObject;

    [SerializeField]
    Button battleButton;

    [SerializeField]
    GameObject[] castles;

    [SerializeField]
    Button[] castleButtons;

    [SerializeField]
    string[] castleNames;

    private string selectedCastle;
    private int selectedCastleID;
    private bool castleSelected = false;
    #endregion

    void Start ()
    {
        ActivateCastleButton(0);
        battleButton.onClick.AddListener(EnterBattle);
        LoadMap();
    }

    // activates castles that have been unlocked
    void LoadMap()
    {
        if (GameData.current == null)
            GameData.current = new GameData();

        int destroyedCastleID = 0;
        foreach(bool castle in GameData.current.destroyedCastles)
        {
            if (castle == true)
            {
                MarkAsDestroyed(destroyedCastleID);
            }
            destroyedCastleID++;
        }
        
    }	

    void MarkAsDestroyed(int castleID)
    {
        if (castleID != 3)
        {
            ActivateCastleButton(castleID+1);
        }
        ActivateCastleButton(castleID);
    }

    void ActivateCastleButton(int castleID)
    {
        castleButtons[castleID].enabled = true;
        castleButtons[castleID].onClick.AddListener(ChooseBattle);
    }

    void ChooseBattle()
    {
        castleSelected = true;
        selectedCastle = EventSystem.current.currentSelectedGameObject.name;

        int currentID = 0;
        foreach (string castle in castleNames)
        {
            if (castle == selectedCastle)
            {
                Debug.Log("Castle " + selectedCastle + " selected");
                selectedCastleID = currentID;
                castleSelected = true;
            }
            currentID++; 
        }
    }

    void EnterBattle()
    {
        Debug.Log("entering battle");
        if (castleSelected)
        {
            battleObject.SetActive(true);
            this.gameObject.SetActive(false);
        }  
    }


}
