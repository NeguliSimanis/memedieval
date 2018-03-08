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
    GameObject[] castles; // game objects with an EnemyBalancer attached

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
        Debug.Log("loading map");
        if (GameData.current == null)
        {
            Debug.Log("no game data object");   
            GameData.current = new GameData();
        }

        int destroyedCastleID = 0;
        foreach(bool castle in GameData.current.destroyedCastles)
        {
            //Debug.Log("checking for destroyed castles");
            if (castle == true)
            {
                MarkAsDestroyed(destroyedCastleID);
            }
            else Debug.Log("castle " + destroyedCastleID + "not destroyed");
            destroyedCastleID++;
        }
        
    }	

    void MarkAsDestroyed(int castleID)
    {
        Debug.Log(castles.Length);

        // destroying first castle unlocks the second and third castles
        if (castleID == 0)
        {
            ActivateCastleButton(castleID + 1);
            ActivateCastleButton(castleID + 2);
        }
            
        // destroying the third or the last castle doesn't unlock anything new
        else if (castleID != 2 && castleID != castles.Length-1)
        {
            Debug.Log("activating next castle (" + castleID + 1 + ")");
            ActivateCastleButton(castleID+1);
        }

        // unlock the destroyed castle and all previous castles    
        for (int i = castleID; i >= 0; i--)
        {
            ActivateCastleButton(castleID);
        }
    }

    void ActivateCastleButton(int castleID)
    {
        castleButtons[castleID].enabled = true;
        castleButtons[castleID].onClick.AddListener(ChooseBattle);
    }

    void ChooseBattle()
    {
        Debug.Log("battle location chosen");
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

        castles[selectedCastleID].SetActive(true);
        EnemyBalancer activatedEnemyCastle = castles[selectedCastleID].GetComponent<EnemyBalancer>();
        // activatedEnemyCastle.enabled = true;
        activatedEnemyCastle.currentCastleID = selectedCastleID;

        if (castleSelected)
        {
            battleObject.SetActive(true);
            this.gameObject.SetActive(false);
        }  
    }


}
