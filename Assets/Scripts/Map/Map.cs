using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // to get the name of the selected button
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour {

    #region variables
    [SerializeField]
    GameObject battleController;

    private string destroyedMarkerName = "DestroyedAnim";
    private Color inactiveElementColor = new Color(0, 0, 0, 1);

    private GameObject selectedCastleMarker = null;
    private string selectedCastleMarkerName = "CastleSelected";

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
        HideUnavailableCastles();
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
                Debug.Log("castle " + destroyedCastleID + " destroyed");
                MarkAsDestroyed(destroyedCastleID);
            }
            //else Debug.Log("castle " + destroyedCastleID + "not destroyed");
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

        // unlock the destroyed castle and all previous castles    
        for (int i = castleID; i >= 0; i--)
        {
            ActivateCastleButton(castleID);
        }

        // destroying first castle unlocks the second and third castles
        if (castleID == 0)
        {
            ActivateCastleButton(castleID + 1);
            ActivateCastleButton(castleID + 2);
        }    
        else if (castleID == 2 || castleID == castles.Length)
        {
            //Debug.Log("castle 2 not destroyed");
            //ActivateCastleButton(castleID + 1);
        }
        // destroying a castle other than the last or third one unlocks the next castle 
        else
        {
            ActivateCastleButton(castleID + 1);
            ActivateCastleButton(castleID + 2);
        }
    }

    void HideUnavailableCastles()
    {
        int i = 0;
        foreach (Button castleButton in castleButtons)
        {
            if (castleButton.enabled == false)
            {
                foreach (Transform child in castleButtonContainers[i].transform)
                {
                    child.gameObject.GetComponent<Image>().color = inactiveElementColor;
                   // child.gameObject.SetActive(true);
                }       
            }
            i++;
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

        // disable castle markers that are not currently selected
        if (selectedCastleMarker != null)
        {
            selectedCastleMarker.SetActive(false);
        }

        // enable selected castle marker
        selectedCastleMarker = castleButtonContainers[selectedCastleID].transform.Find(selectedCastleMarkerName).gameObject;
        selectedCastleMarker.SetActive(true);
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
            // disable enemy spawning in the first battle
            if (selectedCastleID == 0)
            {
                battleController.GetComponent<Testing>().spawnEnemyUnits = false;
            }
            
            castles[selectedCastleID].SetActive(true);
            EnemyBalancer activatedEnemyCastle = castles[selectedCastleID].GetComponent<EnemyBalancer>();
            activatedEnemyCastle.currentCastleID = selectedCastleID;

            battleObject.SetActive(true);
            PlayerProfile.Singleton.ModifyBattleProperties();
            this.gameObject.SetActive(false);
        }  

        else
        {
            battleButtonText.text = noCastleSelected;
        }
    }


}
