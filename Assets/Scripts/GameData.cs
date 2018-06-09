﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script:
 * > stores all data that has to be saved/loaded 
 * > converts the loadable variables to a serializable format
 */

[System.Serializable]
public class GameData
{
    
    public static GameData current;
    public List<ChampionData> championList = new List<ChampionData>();

    #region resources
    // salt
    public int salt = 5;
    public int newChampionCost = 5; // TO-DO: remove; not used everywhere

    // ducats
    public int ducats = 10;
    #endregion

    #region game progress
    public bool[] destroyedCastles = new bool[4];
    #endregion

    #region tags
    // must be the same in the scenes or some scripts might not work
    public string enemyCastleTag = "EnemyCastle";
    public string playerProfileTag = "Player profile"; // TO-DO replace all references to PlayerProfile.Singleton
    public string enemyBalancerTag = "Enemy balancer";
    #endregion

    #region scenes
    public string tavernSceneName = "Tavern";
    public string battleSceneName = "Test scene";
    #endregion
}

[System.Serializable]
public class ChampionData
{

    public int level;   
    public int champClass; // 0 - peasant, 1 - Knight, 2 - archer (in some places peasant and archer are inversed)
    public bool isDead;
    
    public enum Ability { Warhorn, BerserkFury, Prayer, AbilityCount };
    public Ability currentChampionAbility;

    #region recruiting
    public bool isSaltCostSet = false;
    public int saltCost;
    #endregion

    #region Picture stuff
    public bool isCameraPicture = false;
    public byte[] picture;
    private int pictureWidth = 1280;
    private int pictureHeight = 720;
    private int pictureMipmapCount = 11;
    private TextureFormat pictureFormat = TextureFormat.RGBA32;
    #endregion

    #region skills
    public int currentExp = 0;
    public int nextLevelExp = 10;

    public int skillpoints = 1;
    public int charm = 0;
    public int discipline = 0;
    public int brawn = 0;
    public int wisdom = 0;
    public int luck = 0;
    public int wealth = 0;
    #endregion

    #region flair
    public string Name;
    public bool isMan;
    public string bio;
    public string quote;
    #endregion

    public void SetPicture(Texture2D texture)
    {
        pictureHeight = texture.height;
        pictureWidth = texture.width;
        pictureFormat = texture.format;
        picture = texture.GetRawTextureData();
    }

    public Texture2D LoadPictureAsTexture2D()
    {
        Texture2D tex = new Texture2D(pictureWidth, pictureHeight, pictureFormat, false);
        tex.LoadRawTextureData(picture);
        tex.Apply();
        return tex;
    }

    public void ChooseRandomAbility()
    {
        int abilityID = 2;
        abilityID = Random.Range(0, (int)Ability.AbilityCount);
        currentChampionAbility = (Ability)abilityID;
    }

    public string GetChampionClass()
    {
        if (champClass == 0)
        {
            return "Peasant";  
        }
        else if (champClass == 1)
        {
            return "Knight";
        }
        else if (champClass == 2)
        {
            return "Archer";
        }
        Debug.Log("ERROR");
        return "ERROR IR GAMEDATA.GETCHAMPIONCLASS";
    }

    public string GetAbilityString()
    {
        if (currentChampionAbility == Ability.BerserkFury)
        {
            return "Berserk Fury";
        }
        else if (currentChampionAbility == Ability.Prayer)
        {
            return "Prayer";
        }
        else 
        {
            return "Warhorn";
        }
    }

    public int GetSaltCost()
    {
        if (!isSaltCostSet)
        {
            saltCost = Random.Range(3, 6) + level;
            isSaltCostSet = true;
        }
        return saltCost;
    }
}

