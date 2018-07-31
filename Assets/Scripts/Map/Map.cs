using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // to get the name of the selected button
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour {
    #region variables
    [SerializeField]
    SwitchMapButton switchMapButton;

    [SerializeField]
    GameObject mapChildren;

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
    private bool isCastleSelected = false;

    #region selecting champions
    bool isArmyReady = false;
    private string strategyViewScene = "Strategy view";
    #endregion

    bool anyObjectSelected = false;
    string selectedObjectName;

    #region switching map views
    [Header("Switching between map views")]
    [SerializeField]
    SwitchMapButton nextMapSwitch; // component attached to button for switching to 2nd map    
    #endregion
    #endregion

    void Start()
    {
        ActivateCastleButton(0);
        closeMapButton.onClick.AddListener(CloseMap);
        LoadMap();
        HideUnavailableCastles();
        CheckArmyReadiness();
        ShowCorrectMapView();
    }

    // shows the map view where the last destroyed castle was located
    void ShowCorrectMapView()
    {
        // one of first castles was destroyed last - show first view
        if (GameData.current.lastDestroyedCastle < 4)
        {
            switchMapButton.InstantSwitchMap(0);
        }
        // one of latter castles was destroyed - show second view
        else
        {
            switchMapButton.InstantSwitchMap(1);
        }
    }

    void CheckArmyReadiness()
    {
        isArmyReady = false;
        foreach (Champion champion in PlayerProfile.Singleton.champions)
        {
            if (champion.invitedToBattle)
            {
                isArmyReady = true;
                break;
            }
        }
    }

    // activates castles that have been unlocked
    void LoadMap()
    {
        SetCastleButtonContainers();

        // access game data about destroyed castles
        if (GameData.current == null)
        {
            GameData.current = new GameData();
        }

        // check game data for destroyed castles in the first view
        for (int i = 0; i < 8; i++)
        {
            // found a castle that is destroyed
            if (GameData.current.destroyedCastles[i] == true)
            {
                MarkAsDestroyed(i);
            }
            // found a castle that's not destroyed
            else
            {
                // check if the castle can is already available for attacking
                CheckIfCastleUnlocked(i);

                // second castle not destroyed, cannot open next map view
                if (i == 2)
                {
                    HideNextMapButton();
                }
            }
        }
    }

    void HideNextMapButton()
    {
        nextMapSwitch.DisableButton();
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

    void CheckIfCastleUnlocked(int castleID)
    {
        // first castle is destroyed - unlocks the second and third castles
        if (GameData.current.destroyedCastles[0] == true)
        {
            if (castleID == 1 || castleID == 2)
            {
                ActivateCastleButton(castleID);
            }
        }
        // fourth castle is unlocked by destroying the 2nd one
        if (castleID == 3 && GameData.current.destroyedCastles[1] == true)
        {
            ActivateCastleButton(castleID);
        }
        // fifth castle is unlocked by destroying the 3d one
        else if (castleID == 4 && GameData.current.destroyedCastles[2] == true)
        {
            ActivateCastleButton(castleID);
        }
        // all other cases - if previous castle is destroyed, this one is unlocked
        else if (castleID != 4 && castleID != 3)
        {
            if (castleID > 0 && GameData.current.destroyedCastles[castleID - 1] == true)
                ActivateCastleButton(castleID);
        }
    }

    // mark castle as destroyed on map
    void MarkAsDestroyed(int castleID)
    {
        EnableDestroyedCastleMarker(castleID);
        ActivateCastleButton(castleID);
    }

    // recolors inactive castles to black
    void HideUnavailableCastles()
    {
        int i = 0;
        Image castleImageElement;
        foreach (Button castleButton in castleButtons)
        {
            if (castleButton.enabled == false)
            {
                foreach (Transform child in castleButtonContainers[i].transform)
                {
                    castleImageElement = child.gameObject.GetComponent<Image>();
                    // recolor only visible elements
                    if (castleImageElement.color.a > 0)
                    {
                        castleImageElement.color = inactiveElementColor;
                    }
                }       
            }
            i++;
        }      
    }

    void ActivateCastleButton(int castleID)
    {
        if (castleButtons[castleID].enabled == true)
        {
            return; // for avoiding additions of multiple listeners
        }
        castleButtons[castleID].enabled = true;
        castleButtons[castleID].onClick.AddListener(SelectTargetCastle);
    }

    void SelectTargetCastle()
    {
        // castle was already selected -> enter battle or strategy view
        if (isCastleSelected && isBattleShortcutEnabled)
        {
            if (selectedCastle == selectedObjectName)
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
        }

       // isCastleSelected = true;
        if (anyObjectSelected)
            selectedCastle = selectedObjectName;

        int currentID = 0;
        foreach (string castleName in castleNames)
        {
            if (castleName == selectedCastle)
            {
                selectedCastleID = currentID;
                isCastleSelected = true;    
            }
            currentID++;
        }

        // disable castle marker that is not currently selected
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

    public void EnterBattle()
    {
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

    void EnterStrategyView()
    {
        SceneManager.LoadScene(strategyViewScene);
    }

    void ActivateCastleMarker()
    {
        isBattleShortcutEnabled = true;
        selectedCastleMarker = castleButtonContainers[selectedCastleID].transform.Find(selectedCastleMarkerName).gameObject;
        selectedCastleMarker.SetActive(true);
    }

    ////////////// 09.07.2018 //////////////

    public void SelectGameObject(string selectedObject)
    {
        anyObjectSelected = true;
        selectedObjectName = selectedObject;
    }

}
