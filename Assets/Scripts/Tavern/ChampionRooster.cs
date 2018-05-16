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
    GameObject championButtonContainer1;
    [SerializeField]
    Image championPicture1;

    [SerializeField]
    GameObject championButtonContainer2;
    [SerializeField]
    Image championPicture2;

    [SerializeField]
    Text[] championTitles;
    [SerializeField]
    Button[] championButtons;
    int[] championButtonIDs = { 0, 1 };

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
    Image selectedChampionFace;
    [SerializeField]
    Text selectedChampionCost;

    PlayerProfile playerProfile;
    NeutralChampions neutralChampions;
    string neutralChampionsObjectName = "ChampionRooster";

    bool isChampionSelected = false;
    int selectedChampionID = -1;

    void Start()
    {
        InitializeVariables();
        AddButtonListeners();
        DisplayNeutralChampionsList();
        InitializeChampionSelection();
    }

    void AddButtonListeners()
    {
        createChampion.onClick.AddListener(OpenChampionCreation);
        championButtons[0].onClick.AddListener(SelectChampion0);
        championButtons[1].onClick.AddListener(SelectChampion1);
        hireChampionButton.onClick.AddListener(HireSelectedChampion);
    }

    void HireSelectedChampion()
    {
        Champion selectedChampion = neutralChampions.neutralChampionsList[selectedChampionID];
        
        // TO DO - check resources

        // recruit champion
        playerProfile.champions.Add(selectedChampion);
        neutralChampions.neutralChampionsList.Remove(selectedChampion);

        // hide the recruited champion
        HideChampionButtonContainer();
    }

    void HideChampionButtonContainer()
    {
        if (selectedChampionID == 0)
        {
            championButtonContainer1.SetActive(false);
            if (neutralChampions.neutralChampionsList.Count == 0)
            {
                championButtonContainer2.SetActive(false);
            }
        }
        else if (selectedChampionID == 1)
        {
            championButtonContainer2.SetActive(false);
        }
        selectedChampionID = 0;
        DisplaySelectedChampion();
    }

    void SelectChampion0()
    {
        selectedChampionID = 0;
        DisplaySelectedChampion();
    }

    void SelectChampion1()
    {
        if (neutralChampions.neutralChampionsList.Count == 1)
        {
            selectedChampionID = 0;
        }
        else
        {
            selectedChampionID = 1;
        }
        DisplaySelectedChampion();
    }

    void InitializeVariables()
    {
        playerProfile = PlayerProfile.Singleton;
        neutralChampions = playerProfile.gameObject.transform.Find(neutralChampionsObjectName).gameObject.GetComponent<NeutralChampions>();
    }

    void DisplayNeutralChampionsList()
    {
        if (neutralChampions.neutralChampionsList.Count == 0)
        {
            noChampionsPanel.SetActive(true);
            return;
        }
        if (neutralChampions.neutralChampionsList[0] != null)
        {
            DisplayNeutralChampion(0, championButtonContainer1);
        }
        if (neutralChampions.neutralChampionsList[1] != null)
        {
            DisplayNeutralChampion(1, championButtonContainer2);
        }
    }

    void DisplayNeutralChampion(int championID, GameObject currentChampionButton)
    {
        currentChampionButton.SetActive(true);
        Champion currentChampion = neutralChampions.neutralChampionsList[championID];

        championTitles[championID].text = currentChampion.properties.Name;

        #region set champion picture
        Sprite currentChampionSprite = currentChampionButton.transform.GetChild(0).gameObject.GetComponent<Image>().sprite;
        Texture2D newTexture2D = currentChampion.properties.LoadPictureAsTexture2D();

        Sprite newSprite = Sprite.Create(newTexture2D, new Rect(0.0f, 0.0f, newTexture2D.width, newTexture2D.height), new Vector2(0.5f, 0.5f), 100.0f);
        currentChampionSprite = newSprite;

        if (championID == 0)
        {
            championPicture1.sprite = newSprite;
        }
        else
            championPicture2.sprite = newSprite;
        #endregion
    }

    void InitializeChampionSelection()
    {
        if (selectedChampionID == -1 && neutralChampions.neutralChampionsList.Count > 0)
        {
            isChampionSelected = true;
            selectedChampionID = 0;
            DisplaySelectedChampion();
        }
    }

    

    void DisplaySelectedChampion()
    {
        // check if action possible
        if (neutralChampions.neutralChampionsList.Count == 0)
        {
            noChampionsPanel.SetActive(true);
            return;
        }

        // initialize variables
        noChampionsPanel.SetActive(false);
        ChampionData selectedChampionData = neutralChampions.neutralChampionsList[selectedChampionID].properties;
        Texture2D newTexture2D = selectedChampionData.LoadPictureAsTexture2D();
        Sprite newSprite = Sprite.Create(newTexture2D, new Rect(0.0f, 0.0f, newTexture2D.width, newTexture2D.height), new Vector2(0.5f, 0.5f), 100.0f);

        // display name
        selectedChampionName.text = selectedChampionData.Name;

        // display class
        selectedChampionSubtitle.text = "LV " + (selectedChampionData.level + 1) + " " + selectedChampionData.GetChampionClass();

        // display bio
        selectedChampionBio.text = selectedChampionData.bio;

        // display face
        selectedChampionFace.sprite = newSprite;

        // display cost
        selectedChampionCost.text = selectedChampionData.GetSaltCost().ToString();

    }

    void OpenChampionCreation()
    {
        championCreationScreen.SetActive(true);
        championRooster.SetActive(false);
    }
}
