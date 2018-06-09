using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StrategyView : MonoBehaviour {

    [SerializeField] Button closeButton;
    [SerializeField] Button acceptButton;
    private string sceneAfterClose = "Test scene";

    [SerializeField] GameObject championCard;
    [SerializeField] GameObject championLeftPanel;
    [SerializeField] GameObject championRightPanel;

    private GameObject championSelectedMarker;
    private string championSelectedMarkerName = "ChampionSelected";
    private GameObject championIdleMarker;
    private string championIdleMarkerName = "ChampionIdle";

    [SerializeField] Text totalChampionsSelected;

    int currentChampionID;
    int selectedChampionCount = 0;
    int maxAllowedChampionCount = 3;

    void Start()
    {
        UpdateSelectionCount();
        AddButtonListeners();
        LoadChampionList();
        
    }

    void AddButtonListeners()
    {
        closeButton.onClick.AddListener(CloseView);
        acceptButton.onClick.AddListener(CloseView);
    }

    void LoadChampionList()
    {

        currentChampionID = 0;
        bool isLeftPanelAvailable = true;

        foreach (Champion champion in PlayerProfile.Singleton.champions)
        {
            // initialize new champion card
            GameObject newChampionCard = Instantiate(championCard, new Vector3(1, 1, 1), Quaternion.identity);
            if (isLeftPanelAvailable)
            {
                newChampionCard.transform.parent = championLeftPanel.transform;
            }
            else
                newChampionCard.transform.parent = championRightPanel.transform;
            isLeftPanelAvailable = !isLeftPanelAvailable;
            newChampionCard.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);

            // initialize the button component of the card
            newChampionCard.gameObject.name = currentChampionID.ToString();
            newChampionCard.GetComponent<Button>().onClick.AddListener(SelectChampion);

            // initialize button text
            newChampionCard.transform.Find("Text").gameObject.GetComponent<Text>().text = champion.properties.Name;
            newChampionCard.transform.Find("ChampionProperties").gameObject.GetComponent<Text>().text = "LV " + (champion.properties.level + 1) + " "+ champion.GetClassName();
            newChampionCard.transform.Find("ChampionAbility").gameObject.GetComponent<Text>().text = "Ability: " + champion.properties.GetAbilityString();

            // initialize button markers
            if (champion.invitedToBattle)
            {
                newChampionCard.transform.Find(championSelectedMarkerName).gameObject.SetActive(true);
            }

            currentChampionID++;
        }
    }

    void SelectChampion()
    {
        FindChampionMarkers();

        // select
        if (!championSelectedMarker.activeInHierarchy)
        {
            if (selectedChampionCount < maxAllowedChampionCount)
            {
                ChangeChampionState(true);
                championSelectedMarker.SetActive(true);
                championIdleMarker.SetActive(false);
            }
        }

        // deselect champion
        else
        {
            ChangeChampionState(false);
            championSelectedMarker.SetActive(false);
            championIdleMarker.SetActive(true);   
        }
        UpdateSelectionCount();
    }

    void ChangeChampionState(bool isSelected = true)
    {
        int championID = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        PlayerProfile.Singleton.champions[championID].invitedToBattle = isSelected;
    }

    void FindChampionMarkers()
    {
        championIdleMarker = EventSystem.current.currentSelectedGameObject.transform.Find(championIdleMarkerName).gameObject;
        championSelectedMarker = EventSystem.current.currentSelectedGameObject.transform.Find(championSelectedMarkerName).gameObject;
    }

    void CloseView()
    {
        SceneManager.LoadScene(sceneAfterClose);
    }
    
    void UpdateSelectionCount()
    {
        selectedChampionCount = 0;
        foreach (Champion champion in PlayerProfile.Singleton.champions)
        {
            if (champion.invitedToBattle == true)
                selectedChampionCount++;
        }
        totalChampionsSelected.text = "Champions selected: " + selectedChampionCount + "/3";
    }
}
