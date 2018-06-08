using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tutorial : MonoBehaviour {

    // Created on 08.06.2018
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

    #region tutorial strings
    string tutorialStringDefendKing = "Defend the king at all costs!";
    string tutorialStringTapArrows = "TAP on arrows to stop them";
    string tutorialStringTapMoreArrows;
    string tutorialStringTapMoreArrowsTemplate = "TAP on {0} more arrows!";
    string tutorialStringCurses = "Curses! Send in the infantry!";
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
    }

    void TeachArrowBlock()
    {
        currentTutorialText.text = tutorialStringTapArrows;
    }

    public void AddBlockedArrow()
    {
        Debug.Log("added blocked arrow");   
        arrowsToBlock--;
        if (arrowsToBlock == 0)
        {
            return;
        }
        tutorialStringTapMoreArrows = string.Format(tutorialStringTapMoreArrowsTemplate, arrowsToBlock);
        currentTutorialText.text = tutorialStringTapMoreArrows;
    }

}
