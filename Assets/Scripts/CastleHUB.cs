﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CastleHUB : MonoBehaviour {
    //poga ved uz krogu
    public Button TavernButton;
    public Text StatusText;
    private string defaultText="";
	void Start () {
        var p = PlayerProfile.Singleton;
        if(string.IsNullOrEmpty(defaultText))
        {
            defaultText = StatusText.text;
        }
        var s = PlayerProfile.Singleton.lastGameStatus;
        if(s>0)
        {
            StatusText.text = "Most glorious victory, oh great leader. You get 10 SALT! What is Your next action?";
            PlayerProfile.Singleton.SaltCurrent += 10;
        }
        if(s<0)
        {
            StatusText.text = "Valiant defeat! What is Your next action?";
        }
        PlayerProfile.Singleton.lastGameStatus = 0;

        for(int i = 0; i < p.champions.Count; i++)
        {
            var champ = p.champions[i];
            if (champ.properties.isDead)
            {
                p.champions.Remove(champ);
                Destroy(champ.gameObject);
            }
        }
	}
	

    public void onTavernHubButtonClick()
    {
        SceneManager.LoadScene("Tavern");
    }
    public void onBattleClick()
    {
        SceneManager.LoadScene("Main");
    }
}
