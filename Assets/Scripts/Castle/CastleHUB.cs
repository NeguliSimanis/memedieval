using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CastleHUB : MonoBehaviour {

    #region variables
    [SerializeField]
    private Button tavernButton;
    private Text tavernButtonText;
    private string tavernButtonDefaultText = "Tavern";
    private bool canEnterTavern = true;

    [SerializeField]
    private Button battleButton;
    private Text battleButtonText;
    private string battleButtonFinalText = "Let's depart!";
    private bool canEnterBattle = false;

    [SerializeField]
    private Text servantDialogue;
    private string defaultText = "";

    private string battleSceneName;
    private string tavernSceneName;
   
    #region champion selection
    [SerializeField] GameObject ChampSelectButtonPrefab;            
    [SerializeField] GameObject ChampSelect;
    [SerializeField] GameObject championSelectionTextContainer;
    private List<GameObject> _champButtons = new List<GameObject>();

    private Champion currentChampion;
    private int currentChampionID = 0;
    private int championCount;
    private bool isSelectingChampions = false;
   // private bool hasSelectedChampions = false;

    #region dialogue
    private string aye = "Aye";
    private string nay = "Nay";
    private string noChampions = "We have no armies to do battle, my liege.";
    private string noChampionsSelected = "My liege, I implore you to bring more forces to the battlefield";
    private string readyForBattle = "You are well prepared for battle, my liege!";
    private int championDialogueArrayLength = 7;
    private int championDialogueID = -1;
    private string[] championDialogue1 = // [7]
    {
        "Shall I ask ",
        "Will you allow ",
        "Is  ",
        "",
        "Is ",
        "",
        "Shall we summon "
    };
    private string[] championDialogue2 = // [7]
    {
        " to join the battle, my liege?",
        " to prove his worth at battle, my liege?", // TO-DO: change depending on gender
        " invited to the field of battle, my liege?",
        " wishes to accompany you on the battlefield.",
        " worthy to stand by your side in this battle?",
        " offers his sword in your service. Shall you accept, my liege?",  // TO-DO: change depending on gender
        " to our aid, my liege?"
    };
    #endregion
    #endregion
    #endregion

    void Start ()
    {
        championCount = PlayerProfile.Singleton.champions.Count;
        LoadData();
        SetDefaultDialogue();
        SetupButtons();  
    }

    void ChooseChampions()
    {
        if (isSelectingChampions)
            return;
        isSelectingChampions = true;

        championCount = PlayerProfile.Singleton.champions.Count;

        if (championCount == 0)
        {
            servantDialogue.text = noChampions;
            return;
        }
      
        canEnterTavern = false;

        currentChampionID = 0;
        currentChampion = PlayerProfile.Singleton.champions[currentChampionID];
        SetChampionDialogue();

        battleButtonText.text = aye;
        tavernButtonText.text = nay;

        tavernButton.onClick.AddListener(DeclineChampion);
        battleButton.onClick.AddListener(AcceptChampion);
        
    }
	
    void AcceptChampion()
    {
        //Debug.Log(championCount);
        currentChampion.invitedToBattle = true;
        canEnterBattle = true;
        if (currentChampionID < championCount - 1)
        {
            ShowNextChampion();
        }
        else if (championCount == 1)
        {
            CheckBattleReadiness();
        }
        else
        {
            CheckBattleReadiness();
        }
    }

    private void DeclineChampion()
    {
        /*if (hasSelectedChampions) return;*/
        if (currentChampionID < championCount - 1)
        {
            currentChampion.invitedToBattle = false;
            ShowNextChampion();
        }
        else CheckBattleReadiness();
    }

    void CheckBattleReadiness()
    {
        // hasSelectedChampions = true;
        battleButton.onClick.RemoveListener(AcceptChampion);
        tavernButton.onClick.RemoveListener(DeclineChampion);

        if (championCount > 0)
        {
            EnterBattle();
        }
        else
        {
            servantDialogue.text = noChampions;
            canEnterTavern = true;
        }
    }

    void ShowNextChampion()
    {
        currentChampionID++;
        currentChampion = PlayerProfile.Singleton.champions[currentChampionID];
        SetChampionDialogue();
    }

    void SetChampionDialogue()
    {
        int currentDialogueID = Random.Range(0, championDialogueArrayLength-1);
        if (championDialogueID == -1)
        {
            championDialogueID = currentDialogueID;
        }
        // don't allow two identical messages in a row
        else if (championDialogueID == currentDialogueID)
        {
            if (currentDialogueID == championDialogueArrayLength) currentDialogueID--;
            else currentDialogueID++; 
        }

        servantDialogue.text = championDialogue1[currentDialogueID] + currentChampion.properties.Name + championDialogue2[currentDialogueID];
    }

    void SetupButtons()
    {
        tavernButton.onClick.AddListener(EnterTavern);
        battleButton.onClick.AddListener(CheckBattleReadiness);

        tavernButtonText = tavernButton.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        battleButtonText = battleButton.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
    }
    void LoadData()
    {
        if (GameData.current == null)
        {
            GameData.current = new GameData();
        }
        battleSceneName = GameData.current.battleSceneName;
        tavernSceneName = GameData.current.tavernSceneName;

        championDialogueArrayLength = championDialogue1.Length;
        if (championDialogue1.Length != championDialogue2.Length)
            Debug.Log("error");
    }

    private void DisplayChampions()
    {
        servantDialogue.enabled = false;
        championSelectionTextContainer.SetActive(true);

        foreach (Champion champ in PlayerProfile.Singleton.champions)
        {
            var buttonp = Instantiate(ChampSelectButtonPrefab);
            var button = buttonp.GetComponent<Image>();
            button.gameObject.SetActive(true);
            button.transform.SetParent(ChampSelect.transform);
            var rect = new Rect(0, 0, champ.properties.LoadPictureAsTexture2D().width, champ.properties.LoadPictureAsTexture2D().height);
            button.sprite = Sprite.Create(champ.properties.LoadPictureAsTexture2D(), rect, Vector2.zero);

            //buttonp.GetComponent<Button>().onClick.AddListener(() => { ChangeChamp(champ); });
            button.GetComponentInChildren<Text>().text = champ.properties.Name;
            _champButtons.Add(button.gameObject);
        }
    }

    private void EnterTavern()
    {
        if (canEnterTavern)
            SceneManager.LoadScene(tavernSceneName);
    }

    private void EnterBattle()
    {
        SceneManager.LoadScene(battleSceneName);
    }

    void SetDefaultDialogue()
    {
        var p = PlayerProfile.Singleton;
        if (string.IsNullOrEmpty(defaultText))
        {
            defaultText = servantDialogue.text;
        }
        var s = PlayerProfile.Singleton.lastGameStatus;
        if (s > 0)
        {
            servantDialogue.text = "Most glorious victory, oh great leader. You get 5 SALT! What is Your next action?";
            ObtainVictorySpoils();
        }
        if (s < 0)
        {
            servantDialogue.text = "Valiant defeat! What is Your next action?";
        }
        PlayerProfile.Singleton.lastGameStatus = 0;

        for (int i = 0; i < p.champions.Count; i++)
        {
            var champ = p.champions[i];
            if (champ.properties.isDead)
            {
                p.champions.Remove(champ);
                Destroy(champ.gameObject);
            }
        }
    }

    void ObtainVictorySpoils()
    {
        PlayerProfile playerProfile = PlayerProfile.Singleton;

        //salt
        playerProfile.SaltCurrent += 5;

        //ducats
        float obtainDucatChance = playerProfile.gameObject.GetComponent<ChampionEffect>().ducatFindChance;
        Debug.Log("ducat chance: " + obtainDucatChance);
        float ducatRoll = Random.Range(0f, 0.99f);
        Debug.Log("ducat roll: " + ducatRoll);

        if (obtainDucatChance > ducatRoll)
            playerProfile.DucatCurrent++;
    }
}
