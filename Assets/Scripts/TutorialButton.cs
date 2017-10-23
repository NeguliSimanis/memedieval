using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialButton : MonoBehaviour
{

    public int clickCounter = 0;

    [SerializeField] bool isNextButton = true;
    [SerializeField]
    GameObject prevButton;
    [SerializeField]
    GameObject playButton;
    [SerializeField]
    GameObject nextButton;
    [SerializeField]
    GameObject tutorialPanel;
    GameObject child0;



    public void ClickAdd()
    {
        clickCounter++;
        ChangeTutorialState();
    }

    public void ClickRemove()
    {
        if (!isNextButton)
        {
            nextButton.GetComponent<TutorialButton>().clickCounter--;
            nextButton.GetComponent<TutorialButton>().ClickRemove();
        }
        else
            ChangeTutorialState();
    }

    private void ChangeTutorialState()
    {
        switch (clickCounter)
        {
            case 1:
                Debug.Log("tutorial state 1");

                this.gameObject.transform.GetChild(0).GetComponent<Text>().text = "Next";
                prevButton.SetActive(true);
                playButton.SetActive(false);

                child0 = tutorialPanel.transform.GetChild(0).gameObject;
                child0.SetActive(true);
                child0.GetComponent<Text>().text = "Take pictures with your phone to choose your champions.\n \n Champions are your strongest units.";
                tutorialPanel.transform.GetChild(1).gameObject.SetActive(false);
                break;

            case 2:
                Debug.Log("tutorial state 2");
                child0.GetComponent<Text>().text = "There are three unit types:\nPeasants, Knights, and Archers.\n \n Peasants are more effective against knights, knights are more effective against archers, and archers are more effective against peasants.";
                break;

            default:
                Debug.Log("default state");

                this.gameObject.transform.GetChild(0).GetComponent<Text>().text = "Tutorial";

                tutorialPanel.transform.GetChild(1).gameObject.SetActive(true);
                tutorialPanel.transform.GetChild(0).gameObject.SetActive(false);

                prevButton.SetActive(false);
                playButton.SetActive(true);
                clickCounter = 0;

                break;
        }
    }

}
