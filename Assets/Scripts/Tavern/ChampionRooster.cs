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
    GameObject championButton1;
    [SerializeField]
    Image championPicture1;

    [SerializeField]
    GameObject championButton2; 
    [SerializeField]
    Image championPicture2;

    [SerializeField]
    Text[]championTitles;

    [Header("Random champion recruiting")]
    [SerializeField]
    GameObject championInfoPanel;
    [SerializeField]
    Button hireChampionButton; 

    [Header("New Champion Creation")]
    [SerializeField]
    Button createChampion;
    [SerializeField]
    GameObject championCreationScreen;
    
    PlayerProfile playerProfile;
    NeutralChampions neutralChampions;
    string neutralChampionsObjectName = "ChampionRooster";

    void Start ()
    {
        InitializeVariables();
        AddButtonListeners();
        DisplayNeutralChampions();
    }


    void AddButtonListeners()
    {
        createChampion.onClick.AddListener(OpenChampionCreation);
    }

    void InitializeVariables()
    {
        playerProfile = PlayerProfile.Singleton;
        neutralChampions = playerProfile.gameObject.transform.Find(neutralChampionsObjectName).gameObject.GetComponent<NeutralChampions>();
    }

    void DisplayNeutralChampions()
    {
        if (neutralChampions.neutralChampionsList[0] != null)
        {
            DisplayNeutralChampion(0, championButton1);
        }
        if(neutralChampions.neutralChampionsList[1] != null)
        {
            DisplayNeutralChampion(1, championButton2);
        }     
    }
    
    void DisplayNeutralChampion (int championID, GameObject currentChampionButton)
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

    void OpenChampionCreation()
    {
        championCreationScreen.SetActive(true);
        championRooster.SetActive(false);
    }
}
