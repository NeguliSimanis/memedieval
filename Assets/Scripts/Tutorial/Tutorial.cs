using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tutorial : MonoBehaviour {

    // Created on 08.06.2018


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
    GameObject tutorialSetupLeft;
    [SerializeField]
    Text tutorialTextLeft;

    [Header("Dialogue with right image")]
    [SerializeField]
    GameObject tutorialSetupRight;
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
    string tutorialStringWellFought = "Splendid victory, sire!";
    bool isTeachingSummoning = false;

    float firstSpawningCooldown = 1f;
    int firstSpawnLimit = 9;
    int unitsSpawned = 0;
    #endregion

    void Start()
    {
        // Disable dialogue
        dialogueAnimator = tutorialSetupLeft.transform.parent.gameObject.GetComponent<Animator>();
        dialogueAnimator.enabled = false;

        // disable enemy arrows 
        enemyArrowController.shootCastleArrows = false;

        // TODO - play king animation

        // play archer animation (over immediately if player clicks)
        isArcherAppearing = true;


        // TODO - start tutorial when time passes or player taps somewhere
    }

    void Update()
    {
        // load next level if player taps anywhere at the end of tutorial
        if (currentTutorialText.text == tutorialStringWellFought && Input.GetMouseButtonDown(0))
        {
            gameObject.GetComponent<LoadScene>().loadLevel("Castle");
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
        spawnKnightButtonContainer.SetActive(false);
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
            tutorialSetupLeft.SetActive(true);
            tutorialSetupRight.SetActive(false);
        }
        else
        {
            Debug.Log("activating right setup");
            currentTutorialText = tutorialTextRight;
            tutorialSetupRight.SetActive(true);
            tutorialSetupLeft.SetActive(false);
        }
    }

    public void PressAcceptButton()
    {
        if (currentTutorialText.text == tutorialStringDefendKing)
        {
            TeachArrowBlock();     
        }

        else if (isTeachingSummoning)
        {
            ShowUnitSummoning();
        }

        else if (currentTutorialText.text == tutorialStringWellFought)
        {
            gameObject.GetComponent<LoadScene>().loadLevel("Castle");
        }
    }

    void ShowUnitSummoning()
    {
        HideDialogueButton();
        ChooseTutorialSetup(false);
        currentTutorialText.text = tutorialStringCurses;
        currentTutorialText.fontSize = defaultDialogueFontSize;

        // enable spawning player units
        spawnKnightButtonContainer.SetActive(true);
        //spawnKnightUnitButton.SetActive(false);

        SpawnEnemyUnits(true);
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
        CreateTutorialChampion();
        isTeachingSummoning = true;

        // hides the dialogue button
        HideDialogueButton(true); 

        // sets the text
        currentTutorialText.text = tutorialStringArrowsBlocked;
        currentTutorialText.fontSize = 30;
    }

    void CreateTutorialChampion()
    {
        CreateChampion championCreator = PlayerProfile.Singleton.gameObject.transform.Find("ChampionCreate").gameObject.GetComponent<CreateChampion>();
        championCreator.CreateTutorialChampion();
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
                TeachUnitSummoning();
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

    void TutorialEnemySpawner()
    {
        unitsSpawned++;
        SpawnKnight();
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
        ChooseTutorialSetup(true);
        currentTutorialText.fontSize = defaultDialogueFontSize;
        currentTutorialText.text = tutorialStringWellFought;
        spawnKnightButtonContainer.SetActive(false);
        HideDialogueButton(true); // show dialogue button   
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
        gameObject.GetComponent<LoadScene>().loadLevel("Castle");
    }
}
