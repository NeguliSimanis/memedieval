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

    string tutorialStringDefendKing = "Defend the king at all costs!";

    #region arrow blocking sequence
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
    [SerializeField]
    GameObject spawnKnightChampionButton;
   // [SerializeField]
    //GameObject spawnKnightUnitButton;

    string tutorialStringCurses = "Curses! Send in the infantry!";
    string tutorialStringWellFought = "Splendid victory, sire!";
    bool isTeachingSummoning = false;

    float firstSpawningCooldown = 1f;
    int firstSpawnLimit = 9;
    int unitsSpawned = 0;
    #endregion


    int arrowsToBlock = 5;

    void Start()
    {
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
        currentTutorialText.text = tutorialStringCurses;
        currentTutorialText.fontSize = defaultDialogueFontSize;

        // enable spawning player units
        spawnKnightButtonContainer.SetActive(true);
        spawnKnightChampionButton.SetActive(false);

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
            //Debug.Log(playerUnit.gameObject.name);
        }
        //foreach ()
    }
}
