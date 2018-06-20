using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChampionRooster : MonoBehaviour {

    [SerializeField]
    GameObject championRooster;

    [SerializeField]
    GameObject championSelectionPanel;

    [Header("Random champion buttons")]
    [SerializeField]
    GameObject[] championButtonContainers; // championButton parent objects
    [SerializeField]
    Text[] championTitles; // name & surname
    [SerializeField]
    Button[] championButtons; // shows more info about champion

    [Header("Random champion recruiting")]
    [SerializeField]
    GameObject championInfoPanel;
    [SerializeField]
    Button hireChampionButton;
    [SerializeField]
    GameObject noChampionsPanel;

    [Header("New Champion Creation")]
    [SerializeField]
    Button createChampion;
    [SerializeField]
    GameObject championCreationScreen;

    [Header("Currently Selected Champion")]
    [SerializeField]
    Text selectedChampionName;
    [SerializeField]
    Text selectedChampionSubtitle;
    [SerializeField]
    Text selectedChampionBio;
    [SerializeField]
    GameObject selectedChampionFaceContainer;
    [SerializeField]
    Text selectedChampionCost;

    PlayerProfile playerProfile;
    NeutralChampions neutralChampions;
    string neutralChampionsObjectName = "ChampionRooster";

    bool isChampionSelected = false;
    int selectedChampionButtonID = -1;

    void Start()
    {
        InitializeVariables();
        InitializeButtons();
        DisplayNeutralChampionsList();
        InitializeChampionSelection();
    }

    void InitializeButtons()
    {
        // initialize button for champion creation
        createChampion.onClick.AddListener(OpenChampionCreation); // disabled on v 0.1.4

        // initialize buttons that select champion
        InitializeChampionButtons();

        // initialize button for hiring champion
        hireChampionButton.onClick.AddListener(HireSelectedChampion);
    }

    void InitializeChampionButtons()
    {
        int neutralChampionCount = neutralChampions.neutralChampionsList.Count;

        // go through all the champion buttons
        for (int i = 0; i < championButtons.Length; i++)
        {
            // hide all buttons in case they were left active in editor
            championButtonContainers[i].SetActive(false);

            if (i < neutralChampionCount)
            {
                // show button if there's a neutral champion available
                championButtonContainers[i].SetActive(true);

                // add button listener
                int tempID = i;
                championButtons[i].onClick.AddListener(() => SelectChampion(tempID));
            }  
        }
    }

    void SelectChampion(int championButtonID)
    {
        selectedChampionButtonID = championButtonID;
        DisplaySelectedChampion();
    }

    void HireSelectedChampion()
    {
        // load champion data
        int selectedChampionID = GetSelectedChampionID();
        Champion selectedChampion = neutralChampions.neutralChampionsList[selectedChampionID];
        
        // check resources
        if (playerProfile.SaltCurrent < selectedChampion.properties.saltCost)
        {
            return;
        }

        // spend resources
        playerProfile.SaltCurrent = playerProfile.SaltCurrent - selectedChampion.properties.saltCost;

        // recruit champion
        playerProfile.champions.Add(selectedChampion);
        neutralChampions.neutralChampionsList.Remove(selectedChampion);

        // hide the recruited champion
        HideChampionButtonContainer();
    }

    void HideChampionButtonContainer()
    {
        // remove button listener
        championButtons[selectedChampionButtonID].onClick.RemoveListener(() => SelectChampion(selectedChampionButtonID));

        // disable button container
        championButtonContainers[selectedChampionButtonID].SetActive(false);
        selectedChampionButtonID = 0;
        DisplaySelectedChampion();
    }

    void InitializeVariables()
    {
        playerProfile = PlayerProfile.Singleton;
        neutralChampions = playerProfile.gameObject.transform.Find(neutralChampionsObjectName).gameObject.GetComponent<NeutralChampions>();
    }

    void DisplayNeutralChampionsList()
    {
        int neutralChampionCount = neutralChampions.neutralChampionsList.Count;

        // check if there are neutral champions
        if (neutralChampionCount == 0)
        {
            Debug.Log("no neutral champions");
            noChampionsPanel.SetActive(true);
            return;
        }

        // go through the list of champion buttons
        for (int i = 0; i < neutralChampionCount; i++)
        {
            if (i < championButtonContainers.Length)
                LoadChampionButton(i, championButtonContainers[i]);
        }
    }

    // displays champion name, picture on the button
    void LoadChampionButton(int championID, GameObject championButtonContainer)
    {
        Champion currentChampion = neutralChampions.neutralChampionsList[championID];

        championTitles[championID].text = currentChampion.properties.Name;
        ChampionPictureActivator.ActivateChampionPicture(championButtonContainer, currentChampion.properties.GetChampionClass());
    }

    void InitializeChampionSelection()
    {
        if (selectedChampionButtonID == -1 && neutralChampions.neutralChampionsList.Count > 0)
        {
            isChampionSelected = true;
            selectedChampionButtonID = 0;
            DisplaySelectedChampion();
        }
    }

    int GetSelectedChampionID()
    {
        int selectedChampionID = 0;
        int inactiveButtons = 0;
        for (int i = 0; i < championButtons.Length; i++)
        {
            if (i == selectedChampionButtonID)
            {
                selectedChampionID = selectedChampionButtonID - inactiveButtons;
                return selectedChampionID;
            }
            if (championButtonContainers[i].activeInHierarchy == false)
            {
                inactiveButtons++;
            }
        }
        return -1;
    }

    void DisplaySelectedChampion()
    {
        int neutralChampionCount = neutralChampions.neutralChampionsList.Count;

        // check if action possible
        if (neutralChampionCount == 0)
        {
            noChampionsPanel.SetActive(true);
            return;
        }

        // initialize variables
        noChampionsPanel.SetActive(false);
        int selectedChampionID = GetSelectedChampionID();
        ChampionData selectedChampionData = neutralChampions.neutralChampionsList[selectedChampionID].properties;

        // display name
        selectedChampionName.text = selectedChampionData.Name;

        // display class
        selectedChampionSubtitle.text = "LV " + (selectedChampionData.level + 1) + " " + selectedChampionData.GetChampionClass();

        // display bio
        selectedChampionBio.text = selectedChampionData.bio;

        // display face
        ChampionPictureActivator.ActivateChampionPicture(selectedChampionFaceContainer, selectedChampionData.GetChampionClass());
        
        // display cost
        selectedChampionCost.text = selectedChampionData.GetSaltCost().ToString();

    }

    void OpenChampionCreation()
    {
        championCreationScreen.SetActive(true);
        championRooster.SetActive(false);
    }
}
