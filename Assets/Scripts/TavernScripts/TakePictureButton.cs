using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakePictureButton : MonoBehaviour {

    private int clickCounter = 0;

    [SerializeField] Text buttomText;   // child of this button
    [SerializeField] Text tutorialText; // child of a different object

    [SerializeField] string nextLevelText = "To Battle!";
    [SerializeField] string nextLevelToLoad = "Main";
    [SerializeField] float loadNextLevelAfter = 2f;
    [SerializeField] GameObject switchCameraButton;

    [SerializeField] string oneCaptainText = "Excellent choice!";
    [SerializeField] string twoCaptainText = "A true beauty!";
    [SerializeField] string threeCaptainText = "A wise decision!";
    
    void Start()
    {
        switchCameraButton.SetActive(true);
    }

    public void CountButtonClick ()
    {
        clickCounter++;

        switch (clickCounter)
        {
            case 1:
                tutorialText.text = oneCaptainText;
                break;

            case 2:
                tutorialText.text = twoCaptainText;
                break;

            case 3:
                tutorialText.text = threeCaptainText;

                // change button text
                buttomText.text = nextLevelText;

                // delete switch camera button
                switchCameraButton.SetActive(false);
                break;

            default:
                // 3 pictures taken, load next level
                StartCoroutine(LoadNextLevel());
                break;
        }
	}
	
    private IEnumerator LoadNextLevel()
    {
        LoadScene loadNextLevel = gameObject.GetComponent<LoadScene>();
        yield return new WaitForSeconds(loadNextLevelAfter);
        loadNextLevel.loadLevel(nextLevelToLoad);
    }
}
