using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // to get the name of the selected button
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour {
    #region variables
    
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
    #endregion

    void Start()
    {
        ActivateCastleButton(0);
        closeMapButton.onClick.AddListener(CloseMap);
        LoadMap();
        HideUnavailableCastles();
        CheckArmyReadiness();
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

    public void UnselectCastle(string castleName)
    {
        // TODO add this to buttons
    }

    public void SelectGameObject(string selectedObject)
    {
        anyObjectSelected = true;
        selectedObjectName = selectedObject;
    }

}
