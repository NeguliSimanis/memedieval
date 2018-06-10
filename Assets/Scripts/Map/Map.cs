using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // to get the name of the selected button
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour {

    [SerializeField]
    GameObject mapChildren;

    #region variables
    bool isBattleShortcutEnabled = false;

    [SerializeField]
    GameObject battleController;

    private string destroyedMarkerName = "DestroyedAnim";
    private Color inactiveElementColor = new Color(0, 0, 0, 1);

    private GameObject selectedCastleMarker = null;
    private string selectedCastleMarkerName = "CastleSelected";

    [SerializeField]
    Button closeMapButton;
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

    public static string selectedCastle;
    public static int selectedCastleID;
    public static bool isCastleSelected = false;

    #region battle button
    [SerializeField] Button battleButton;
    [SerializeField] GameObject battleButtonActiveImage;
    [SerializeField] Text battleButtonText;

    private string selectCastle = "Select castle";
    private string noCastleSelected = "No castle selected!";
    #endregion

    #region selecting champions
    bool isArmyReady = false;
    private string strategyViewScene = "Strategy view";

    [SerializeField]
    Button selectChampionsButton;
    private string selectChampionsButtonDefaultText = "Select Champions";
    private string selectChampionsButtonSelectedText = "Change Army";
    #endregion
    #endregion

    void Start()
    {
        ActivateCastleButton(0);
        AddButtonListeners();
        LoadMap();
        HideUnavailableCastles();
        CheckArmyReadiness();

        if (isArmyReady && isCastleSelected)
        {
            SelectTargetCastle();
        }
    }

    void CheckArmyReadiness()
    {
        foreach (Champion champion in PlayerProfile.Singleton.champions)
        {
            if (champion.invitedToBattle)
            {
                isArmyReady = true;
                break;
            }
        }
    }

    void AddButtonListeners()
    {
        selectChampionsButton.onClick.AddListener(EnterStrategyView);
        battleButton.onClick.AddListener(EnterBattle);
        closeMapButton.onClick.AddListener(CloseMap);
    }

    // activates castles that have been unlocked
    void LoadMap()
    {
        if (!isArmyReady)
            HideBattleButton();

        SetCastleButtonContainers();

        // access game data about destroyed castles
        if (GameData.current == null)
        {
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
            destroyedCastleID++;
        }

        if (isCastleSelected)
            ActivateCastleMarker();
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
        else if (castleID == 2 || castleID == castles.Length-1)
        {
            
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
        // castle was already selected -> enter battle or strategy view
        if (isBattleShortcutEnabled && selectedCastle == EventSystem.current.currentSelectedGameObject.name)
        {
            CheckArmyReadiness();
            if (isArmyReady)
            {
                EnterBattle();
            }
            else
            {
                EnterStrategyView();
            }
        }

        isCastleSelected = true;
        if (EventSystem.current.currentSelectedGameObject != null)
            selectedCastle = EventSystem.current.currentSelectedGameObject.name;

        int currentID = 0;
        foreach (string castle in castleNames)
        {
            if (castle == selectedCastle)
            {
                selectedCastleID = currentID;
                isCastleSelected = true;
                ActivateBattleButton();
            }
            currentID++;
        }

        // disable castle marker that are not currently selected
        if (selectedCastleMarker != null)
        {
            selectedCastleMarker.SetActive(false);
        }

        // enable selected castle marker
        ActivateCastleMarker();
    }

    void EnableDestroyedCastleMarker(int castleID)
    {
        castleButtonContainers[castleID].transform.Find(destroyedMarkerName).gameObject.SetActive(true);
    }

    void ActivateBattleButton()
    {
        if (isArmyReady)
            battleButton.gameObject.SetActive(true);
    }

    void EnterBattle()
    {
        if (isCastleSelected)
        {
            // disable enemy spawning in the first battle
            if (selectedCastleID == 0)
            {
                battleController.GetComponent<Testing>().spawnEnemyUnits = true;
            }
            
            castles[selectedCastleID].SetActive(true);
            EnemyBalancer activatedEnemyCastle = castles[selectedCastleID].GetComponent<EnemyBalancer>();
            activatedEnemyCastle.currentCastleID = selectedCastleID;

            battleObject.SetActive(true);
            PlayerProfile.Singleton.ModifyBattleProperties();
            mapChildren.SetActive(false);
        }  

        else
        {
            battleButtonText.text = noCastleSelected;
        }
    }

    void HideBattleButton()
    {
        battleButton.gameObject.SetActive(false);
    }

    void EnterStrategyView()
    {
        SceneManager.LoadScene(strategyViewScene);
    }

    void ActivateCastleMarker()
    {
        EnableBattleShortcut();
        selectedCastleMarker = castleButtonContainers[selectedCastleID].transform.Find(selectedCastleMarkerName).gameObject;
        selectedCastleMarker.SetActive(true);
    }
     
    void EnableBattleShortcut()
    {
        isBattleShortcutEnabled = true;
        //EnterBattle();
    }
}
