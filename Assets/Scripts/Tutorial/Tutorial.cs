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
    string tutorialStringTapMoreArrowsTemplate = "TAP on {0} more arrows!";
    string tutorialStringTapOnLastArrow = "TAP on 1 more arrow!";
    string tutorialStringArrowsBlocked = "Well done sire, the enemy has wasted their arrows!";
    #endregion

    #region summoning units sequence
    string tutorialStringCurses = "Curses! Send in the infantry!";
    bool isTeachingSummoning = false;
    //string tutorialStringRegularUnit = "";
    #endregion


    int arrowsToBlock = 5;

    void Start()
    {
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

        if (isTeachingSummoning)
        {
            ShowUnitSummoning();
        }
    }

    void ShowUnitSummoning()
    {
        HideDialogueButton();
        currentTutorialText.text = tutorialStringCurses;
        currentTutorialText.fontSize = defaultDialogueFontSize;
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

        // hides the dialogue button
        HideDialogueButton(true); 

        // sets the text
        currentTutorialText.text = tutorialStringArrowsBlocked;
        currentTutorialText.fontSize = 30;
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

}
