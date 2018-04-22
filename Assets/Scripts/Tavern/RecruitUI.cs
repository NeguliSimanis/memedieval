using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecruitUI : MonoBehaviour {
    [SerializeField]
    GameObject championCreateContainer;
    CreateChampion championCreate;

    [SerializeField]
    Button nameAcceptButton;
    [SerializeField]
    GameObject nameCreatePanel;
    [SerializeField]
    GameObject nameCreateButtonPanel;

    [SerializeField]
    GameObject genderCreatePanel;
    [SerializeField]
    Button genderAcceptButton;
    [SerializeField]
    GameObject genderCreateButtonPanel;
    [SerializeField]
    GameObject genderCreateBackground;
    [SerializeField]
    GameObject genderBottomFrame;

    [Header("Face creation")]
    [SerializeField]
    GameObject faceCreatePanel;
    [SerializeField]
    Button faceAcceptButton;
    [SerializeField]
    Button switchCameraButton;
    [SerializeField]
    GameObject faceCreateButtonPanel;
    [SerializeField]
    GameObject defaultFaceObject;

    [SerializeField]
    GameObject saveChampionButtonPanel;
    [SerializeField]
    GameObject saveChampionBackground;
    [SerializeField]
    Button[] endRecruitButtons;


    void Start()
    {
        ResetUI();
        
        nameAcceptButton.onClick.AddListener(SaveName);
        genderAcceptButton.onClick.AddListener(SaveGender);
        faceAcceptButton.onClick.AddListener(SaveFace);
        
        foreach (Button button in endRecruitButtons)
        {
            button.onClick.AddListener(ResetUI);
        }
    }

    void ResetUI()
    {
        championCreate = championCreateContainer.GetComponent<CreateChampion>();
        championCreate.hasPicture = false;

        faceCreatePanel.SetActive(true);
        faceCreateButtonPanel.SetActive(true);

        nameCreatePanel.SetActive(false);
        nameCreateButtonPanel.SetActive(true);

        genderCreatePanel.SetActive(false);
        genderCreateButtonPanel.SetActive(true);
        genderCreateBackground.SetActive(false);
        genderBottomFrame.SetActive(true);

        saveChampionBackground.SetActive(false);
        saveChampionButtonPanel.SetActive(false);

        if (championCreate.GetDeviceLength() <= 1)
        {
            switchCameraButton.gameObject.SetActive(false);
        }
        if (championCreate.GetDeviceLength() == 0)
        {
            defaultFaceObject.SetActive(true);
        }
    }

    void SaveFace()
    {
        //faceCreatePanel.SetActive(false);
        faceCreateButtonPanel.SetActive(false);
        nameCreatePanel.SetActive(true);
    }

    void SaveName()
    {
        nameCreateButtonPanel.SetActive(false);
        genderCreatePanel.SetActive(true);
        genderCreateBackground.SetActive(true);
    }

    void SaveGender()
    {
        genderCreateButtonPanel.SetActive(false);
        saveChampionButtonPanel.SetActive(true);
        genderBottomFrame.SetActive(false);
        saveChampionBackground.SetActive(true);
        //faceCreatePanel.SetActive(true);
    }



}
