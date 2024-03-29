﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchMapButton : MonoBehaviour
{

    /// <summary>
    /// Shows a different map screen when you press the button
    /// 
    /// 23.07.2018 Sīmanis Mikoss
    /// </summary>

    [SerializeField]
    private int buttonID; // used to determine if the button is from the 1st map view or other
    [SerializeField]
    private bool isNextButton;
    private bool isButtonAvailable = true;

    private Image thisButtonImage;
    private Color inactiveButtonColor = new Color(0, 0, 0, 1);
    private Color defaultButtonColor;

    [SerializeField]
    private GameObject[] mapViews; // contain various castle buttons
    private float switchDuration = 0.17f; // how much time passes between clicking button and switching to other map view

    [SerializeField]
    private Text mapTitle;

    void Start ()
    {
        GetButtonImageComponent();
        defaultButtonColor = thisButtonImage.color;
        inactiveButtonColor = new Color(0, 0, 0, 1);
        if (MapData.current == null)
            MapData.current = new MapData();
        SetMapTitle();
    }
	
    void GetButtonImageComponent()
    {
        thisButtonImage = gameObject.GetComponent<Image>();
    }

	public void DisableButton(bool isDisabled = true)
    {
        isButtonAvailable = !isDisabled;
   
        if (thisButtonImage == null)
        {
            GetButtonImageComponent();
        }
        // Change button color to black if it's inactive
        thisButtonImage.color = inactiveButtonColor;
    }

    public void StartSwitchMap()
    {
        //
        if (!isButtonAvailable)
         {
             DisplayHelpMessage();
             return;
         }
        StartCoroutine(SwitchMapAfterSeconds());
        /*/
        StartCoroutine(SwitchMapAfterSeconds());
        //*/
    }

    public IEnumerator SwitchMapAfterSeconds()
    {
        yield return new WaitForSeconds(switchDuration);
        // switching from first view to second view
        if (isNextButton)
        {
            mapViews[buttonID + 1].SetActive(true);
        }
        // switching from second view to first view
        else
        {
            mapViews[buttonID - 1].SetActive(true);       
        }
        mapViews[buttonID].SetActive(false);
        SetMapTitle();
    }

    // called from map script
    public void InstantSwitchMap(int mapViewID)
    {
        for (int i = 0; i < mapViews.Length; i++)
        {
            if (i == mapViewID)
            {
                mapViews[i].SetActive(true);
            }
            else
            {
                mapViews[i].SetActive(false);
            }
        }
        SetMapTitle();
    }

    void SetMapTitle()
    {
        for (int i = 0; i < mapViews.Length; i++)
        {
            if(mapViews[i].activeInHierarchy)
            {
                mapTitle.text = MapData.current.mapRegionNames[i];
            }
        }
    }

    void DisplayHelpMessage()
    {
        gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text = "Locked!";
    }
}
