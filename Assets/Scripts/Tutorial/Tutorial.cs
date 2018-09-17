using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tutorial : MonoBehaviour {

    #region variables
    // Created on 08.06.2018
    GameObject pierre;

    Champion tutorialChampion;
    Image tutorialChampionFace;

    int defaultDialogueFontSize;
    [SerializeField]
    Text currentTutorialText;

    [SerializeField]
    Testing enemyArrowController;
    [SerializeField]
    EnemyCastleController enemyUnitSpawner;

    #region dialogue setups
    [Header("Dialogue with left image")]
    [SerializeField]
    GameObject []tutorialSetupLeft;
    [SerializeField]
    Text tutorialTextLeft;

    [Header("Dialogue with right image")]
    [SerializeField]
    GameObject [] tutorialSetupRight;
    [SerializeField]
    Text tutorialTextRight;
    #endregion

    #region intro sequence
    [Header("Intro Animation")]
    [SerializeField] GameObject enemyArcher;
    [SerializeField] float archerMoveSpeed;
    [SerializeField] GameObject archerDestination;
    Animator dialogueAnimator;
    bool isArcherAppearing = false;
    bool archerAppeared = false;
    #endregion

    string tutorialStringDefendKing = "Defend the king at all costs!";

    #region arrow blocking sequence
    int arrowsToBlock = 5;
    string tutorialStringTapArrows = "TAP on arrows to break them";
    string tutorialStringTapMoreArrows;
    string tutorialStringTapMoreArrowsTemplate = "Break {0} more arrows!";
    string tutorialStringTapOnLastArrow = "Break 1 more arrow!";
    string tutorialStringArrowsBlocked = "Well done sire, the enemy has wasted their arrows!";
    #endregion

    #region summoning units sequence
    [Header("Unit Spawning")]
    [SerializeField]
    GameObject spawnKnightButtonContainer;

    string tutorialStringCurses = "Curses! Send in the infantry!";
    string tutorialStringWorryNotSire = "Worry not, sire! My knights will defend the king!";
    string tutorialStringWellFought = "Splendid victory, sire!";
    bool isTeachingSummoning = false;
    bool waitingForEnemyToSpeak = false;
    bool waitingForChampionToSpeak = false;
    bool waitingForSummoningOptions = false;

    float firstSpawningCooldown = 1f;
    int firstSpawnLimit = 9;
    int unitsSpawned = 0;
    #endregion

    string levelAfterTutorial = "Test scene";
    #endregion
    void Start()
    {
        // display champion spawning buttons
        spawnKnightButtonContainer.SetActive(false);

        // Disable dialogue
        dialogueAnimator = tutorialSetupLeft[0].transform.parent.gameObject.GetComponent<Animator>();
        dialogueAnimator.enabled = false;

        // disable enemy arrows 
        enemyArrowController.shootCastleArrows = false;

        // play archer animation (over immediately if player clicks)
        isArcherAppearing = true;

        CreateTutorialChampion();
    }

    void Update()
    {
        // load next level if player taps anywhere at the end of tutorial
        if (currentTutorialText.text == tutorialStringWellFought && Input.GetMouseButtonDown(0))
        {
            LoadLevelAfterTutorial();
        }

        // move archer during the intro
        if (isArcherAppearing)
        {
            MoveArcher();

            // increase archer speed during the intro if the player taps anywhere
            if (Input.GetMouseButtonDown(0))
            {
                archerMoveSpeed += archerMoveSpeed;
            }
        }

        // start the turoial after archer has moved to position 
        else if (!archerAppeared)
        {
            enemyArrowController.shootCastleArrows = true;
            StartTutorial();
            archerAppeared = true;
        }
    }

    void MoveArcher()
    {
        enemyArcher.transform.position = Vector3.MoveTowards(enemyArcher.transform.localPosition, archerDestination.transform.localPosition, archerMoveSpeed * Time.deltaTime);
        if (enemyArcher.transform.localPosition == archerDestination.transform.localPosition)
        {
            isArcherAppearing = false;
        }
    }

    void StartTutorial()
    {
        dialogueAnimator.enabled = true;
        InitializeVariables();
        ChooseTutorialSetup(true);
        currentTutorialText.text = tutorialStringDefendKing;
    }

    void InitializeVariables()
    {
        defaultDialogueFontSize = currentTutorialText.fontSize;
    }

    void ChooseTutorialSetup(bool isLeftSetup)
    {
        if (isLeftSetup)
        {
            currentTutorialText = tutorialTextLeft;
            EnableRightSetup(false);
            EnableLeftSetup(true);
        }
        else
        {
            currentTutorialText = tutorialTextRight;
            EnableRightSetup(true); 
            EnableLeftSetup(false); 
        }
    }

    void EnableLeftSetup(bool setActive)
    {
        int objectCount = tutorialSetupLeft.Length;
        for (int i = 0; i < objectCount; i++)
        {
            tutorialSetupLeft[i].SetActive(setActive);
        }
    }

    void EnableRightSetup(bool setActive)
    {
        int objectCount = tutorialSetupRight.Length;
        for (int i = 0; i < objectCount; i++)
        {
            tutorialSetupRight[i].SetActive(setActive);
        }
    }

    public void PressAcceptButton()
    {
        if (currentTutorialText.text == tutorialStringDefendKing)
        {
            TeachArrowBlock();     
        }
        else if (waitingForEnemyToSpeak)
        {
            DisplayEnemyDialogue();
        }
        else if (waitingForChampionToSpeak)
        {
            DisplayChampionDialogue();
        }
        else if (waitingForSummoningOptions)
        {
            ShowUnitSummoning();
        }
        else if (currentTutorialText.text == tutorialStringWellFought)
        {
            LoadLevelAfterTutorial();
        }
    }

    void DisplayEnemyDialogue()
    {
        waitingForEnemyToSpeak = false;
        waitingForChampionToSpeak = true;

        // enable enemy dialogue game objects
        ChooseTutorialSetup(false);

        // set enemy dialogue text
        currentTutorialText.text = tutorialStringCurses;
        currentTutorialText.fontSize = defaultDialogueFontSize;

        SpawnEnemyUnits(true);
    }
   
    void DisplayChampionDialogue(bool display = true)
    {
        if (display)
        {
            waitingForChampionToSpeak = false;
            waitingForSummoningOptions = true;

            // activate the left dialogue setup game objects
            ChooseTutorialSetup(true);

            HidePierre(true);

            // display champion dialogue text
            currentTutorialText.text = tutorialStringWorryNotSire;

            // display champion face
            if (tutorialChampionFace == null)
                tutorialChampionFace = tutorialSetupLeft[0].transform.parent.Find("Champion").GetComponent<Image>();
            tutorialChampionFace.enabled = true;

            // display champion name
            string championTitle = tutorialChampion.properties.GetTitle() + " " + tutorialChampion.properties.GetFirstName();
            SetLeftNameTagText(championTitle);
        }
        else
        {
            // hide champion face
            tutorialChampionFace.enabled = false;
        }
    }

    void SetLeftNameTagText(string nameTagText)
    {
        foreach (GameObject gameObject in tutorialSetupLeft)
        {
            if (gameObject.name == "LeftNametag")
            {
                gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text = nameTagText;
            }
        }
    }

    void ShowUnitSummoning()
    {
        //HideDialogueButton();
        dialogueAnimator.SetBool("isOver", true);
        spawnKnightButtonContainer.SetActive(true);
    }

    void SpawnEnemyUnits(bool isSpawning)
    {
        enemyArrowController.spawnEnemyUnits = isSpawning;
        enemyUnitSpawner.spawnEnemies = false;
        InvokeRepeating("TutorialEnemySpawner", 0.5f, firstSpawningCooldown);
    }

    void TeachArrowBlock()
    {
        HideDialogueButton();
        currentTutorialText.transform.parent.Find("AcceptButton").gameObject.SetActive(false);
        currentTutorialText.text = tutorialStringTapArrows;
    }

    void HideDialogueButton(bool hideValue = false)
    {
        currentTutorialText.transform.parent.Find("AcceptButton").gameObject.SetActive(hideValue);
    }

    void TeachUnitSummoning()
    {
        isTeachingSummoning = true;
    }

    void CreateTutorialChampion()
    {
        CreateChampion championCreator = PlayerProfile.Singleton.gameObject.transform.Find("ChampionCreate").gameObject.GetComponent<CreateChampion>();
        tutorialChampion = championCreator.CreateTutorialChampion();
    }

    void StopEnemyArrows()
    {
        enemyArrowController.shootCastleArrows = false;
    }

    public void AddBlockedArrow()
    {
        arrowsToBlock--;
        if (arrowsToBlock <= 0)
        {
            StopEnemyArrows();
            if (isTeachingSummoning == false)
                CongratulateOnBlockingArrows();
            return;
        }
        else if (arrowsToBlock == 1)
        {
            currentTutorialText.text = tutorialStringTapOnLastArrow;
            return;
        }
        tutorialStringTapMoreArrows = string.Format(tutorialStringTapMoreArrowsTemplate, arrowsToBlock);
        currentTutorialText.text = tutorialStringTapMoreArrows;
        HideDialogueButton();
    }

    // happens when you have blocked all the arrows
    void CongratulateOnBlockingArrows()
    {
        //CreateTutorialChampion();

        // shows player dialogue button
        HideDialogueButton(true);

        // sets the congratulation text
        currentTutorialText.text = tutorialStringArrowsBlocked;
        currentTutorialText.fontSize = 30;

        waitingForEnemyToSpeak = true;
    }   

    void TutorialEnemySpawner()
    {
        unitsSpawned++;
        SpawnKnight();
    }

    void HidePierre(bool hide = true)
    {
        // find Pierre
        if (pierre == null)
        {
            foreach (GameObject gameObject in tutorialSetupLeft)
            {
                if (gameObject.name == "Pierre")
                    pierre = gameObject;
            }
        }
        // hide/show Pierre
        pierre.SetActive(!hide);

        // reset nametag when Pierre appears again
        if (!hide)
        {
            SetLeftNameTagText("Pierre");
        }
    }

    void SpawnKnight()
    {
        if (unitsSpawned > firstSpawnLimit)
        {
            return;
        }
        enemyUnitSpawner.SpawnTutorialKnight();
    }

    public void DefeatTutorialEnemy()
    {
        CelebrateVictory();
        isTeachingSummoning = false;

        // show/hide the characters
        HidePierre(false);
        DisplayChampionDialogue(false);
        
        // bring up the dialogue panel
        dialogueAnimator.SetBool("isVictory", true);

        // enable game objects for left tutorial setup
        ChooseTutorialSetup(true);

        // setup victory dialogue texts
        currentTutorialText.fontSize = defaultDialogueFontSize;
        currentTutorialText.text = tutorialStringWellFought;

        // disable unit spawning
        spawnKnightButtonContainer.SetActive(false);

        // show dialogue button 
        HideDialogueButton(true);   
    }

    private void CelebrateVictory()
    {
        var playerUnits = FindObjectsOfType<PlayerUnit>();
        foreach (PlayerUnit playerUnit in playerUnits)
        {
            playerUnit.gameObject.GetComponent<WaypointFollower>().Celebrate();
        }
    }

    public void SkipTutorial()
    {
        CreateTutorialChampion();
        LoadLevelAfterTutorial();
    }

    private void LoadLevelAfterTutorial()
    {
        gameObject.GetComponent<LoadScene>().loadLevel(levelAfterTutorial);
    }
}
