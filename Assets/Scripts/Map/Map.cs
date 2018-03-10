using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // to get the name of the selected button
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour {

    #region variables
    private string destroyedMarkerName = "DestroyedAnim";

    [SerializeField]
    Button CloseMapButton;
    private string sceneAfterCloseMap = "Castle";

    [SerializeField]
    GameObject battleObject;

    [SerializeField]
    GameObject[] castles; // game objects with an EnemyBalancer attached

    [SerializeField]
    Button[] castleButtons;
    private List<GameObject> castleButtonContainers;

    [SerializeField]
    string[] castleNames;

    private string selectedCastle;
    private int selectedCastleID;
    private bool castleSelected = false;

    #region battle button
    [SerializeField] Button battleButton;
    [SerializeField] GameObject battleButtonActiveImage;
    [SerializeField] Text battleButtonText;

    private string selectCastle = "Select castle";
    private string noCastleSelected = "No castle selected!";
    #endregion

    #endregion

    void Start()
    {
        ActivateCastleButton(0);
        battleButton.onClick.AddListener(EnterBattle);
        CloseMapButton.onClick.AddListener(CloseMap);
        LoadMap();
    }

    // activates castles that have been unlocked
    void LoadMap()
    {
        // tell player to select target castle
        battleButtonText.text = selectCastle;

        SetCastleButtonContainers();

        // access game data about destroyed castles
        if (GameData.current == null)
        {
            Debug.Log("no game data object");
            GameData.current = new GameData();
        }

        // check for destroyed castles
        int destroyedCastleID = 0;
        foreach (bool castle in GameData.current.destroyedCastles)
        {
            if (castle == true)
            {
                MarkAsDestroyed(destroyedCastleID);
            }
            else Debug.Log("castle " + destroyedCastleID + "not destroyed");
            destroyedCastleID++;
        }

    }

    void SetCastleButtonContainers()
    {
        castleButtonContainers = new List<GameObject>();
        foreach (Button castleButton in castleButtons)
        {
            castleButtonContainers.Add(castleButton.gameObject.transform.parent.gameObject);
        }
    }

    void CloseMap()
    {
        SceneManager.LoadScene(sceneAfterCloseMap);
    }

    void MarkAsDestroyed(int castleID)
    {
        EnableDestroyedCastleMarker(castleID);

        // destroying first castle unlocks the second and third castles
        if (castleID == 0)
        {
            ActivateCastleButton(castleID + 1);
            ActivateCastleButton(castleID + 2);
        }

        // destroying a castle other than the last or third one unlocks the next castle 
        else if (castleID != 2 && castleID != castles.Length - 1)
        {
            //Debug.Log("activating next castle (" + castleID + 1 + ")");
            ActivateCastleButton(castleID + 1);
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
        castleButtons[castleID].onClick.AddListener(SelectTargetCastle);

    }

    void SelectTargetCastle()
    {
        castleSelected = true;
        selectedCastle = EventSystem.current.currentSelectedGameObject.name;

        int currentID = 0;
        foreach (string castle in castleNames)
        {
            if (castle == selectedCastle)
            {
               // Debug.Log("Castle " + selectedCastle + " selected");
                selectedCastleID = currentID;
                ActivateBattleButton();
            }
            currentID++;
        }    
    }

    void EnableDestroyedCastleMarker(int castleID)
    {
        castleButtonContainers[castleID].transform.Find(destroyedMarkerName).gameObject.SetActive(true);
    }

    void ActivateBattleButton()
    {
        castleSelected = true;

        battleButtonActiveImage.SetActive(true);
        battleButtonText.enabled = false;
    }

    void EnterBattle()
    {
        if (castleSelected)
        {
            //Debug.Log("entering battle");

            castles[selectedCastleID].SetActive(true);
            EnemyBalancer activatedEnemyCastle = castles[selectedCastleID].GetComponent<EnemyBalancer>();
            activatedEnemyCastle.currentCastleID = selectedCastleID;

            battleObject.SetActive(true);
            this.gameObject.SetActive(false);
        }  

        else
        {
            battleButtonText.text = noCastleSelected;
        }
    }


}
