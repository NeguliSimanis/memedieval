using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// For changing the button images of captains. 
/// The captain unit images are changed in Spawn.cs
/// 
/// is attached to champion buttons in ChampionPanel in Main scene
/// </summary>

public class ChangeCaptainImage : MonoBehaviour
{
    [SerializeField] int captainClass; // 1 - peasant, 2 - knight, 3 - archer
    private GameObject captainFace;
    private GameObject thisCaptainFace;
    private GameObject gameManager;
    private Sprite captainFaceImage;

    void Start ()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game manager");
        gameManager.GetComponent<AssignCaptainFace>().AwakeChildren();

        switch (captainClass)
        {
            case 1:
                captainFace = GameObject.FindWithTag("Peasant face");
                break;

            case 2:
                captainFace = GameObject.FindWithTag("Knight face");
                break;

            case 3:
                captainFace = GameObject.FindWithTag("Archer face");
                break;
        }

        thisCaptainFace = Instantiate(captainFace, this.gameObject.transform);
        thisCaptainFace.transform.localScale += new Vector3(9F, 9f, 9f);
        thisCaptainFace.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
        gameObject.GetComponent<Image>().sprite = captainFace.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite;


        //captainFaceImage.Im
    }
	
}
