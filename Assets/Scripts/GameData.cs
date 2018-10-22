using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  * Script:
///  > stores all data that has to be saved/loaded
///  > converts the loadable variables to a serializable format
/// </summary>
[System.Serializable]
public class GameData
{
    public static GameData current;
    public List<ChampionData> championList = new List<ChampionData>();

    #region settings
    public bool soundMuted = true;
    #endregion

    #region resources
    // salt
    public int salt = 5;
    public int newChampionCost = 5; // TO-DO: remove; not used everywhere

    // ducats
    public int ducats = 10;
    #endregion

    #region game progress
    public bool[] destroyedCastles = new bool[8];
    public int lastDestroyedCastle = -1;
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

    #region misc
    public int lastChampionID = 0;
    #endregion

    public void LoadGameProgress(GameData previousProgress)
    {
        if (GameData.current == null)
            GameData.current = new GameData();
        GameData.current = previousProgress;
    }
    /// <summary>
    /// Loops through all castles and returns the highest ID of a destroyed castle
    /// </summary>
    /// <returns></returns>
    public int HighestDestroyedCastleID()
    {
        int highestCastleID = -1;
        for (int i = 0; i < destroyedCastles.Length; i++)
        {
            if (destroyedCastles[i] == true && i > highestCastleID)
            {
                highestCastleID = i;
            }
        }
        return highestCastleID;
    }

    public int GetNewChampionID()
    {
        lastChampionID++;
        return lastChampionID;
    }
}

[System.Serializable]
public class ChampionData
{
    public int championID = -1;
    public int level; // actual level is 1 higher
    public int champClass; // 0 - peasant, 1 - Knight, 2 - archer (in some places peasant and archer are inversed)
    public bool isDead;
    
    public enum Ability { Warhorn, BerserkFury, Prayer, AbilityCount };
    public Ability currentChampionAbility;

    #region recruiting
    public bool isSaltCostSet = false;
    public int saltCost;
    #endregion

    #region Crest
    public bool crestSet = false;
    public int crestColorID;
    public int crestPatternID;
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

    public void SetID()
    {
        CreateGameData.CreateIfNoGameDataExists();
        championID = GameData.current.GetNewChampionID();
    }
    
    public Color GetCrestColor()
    {
        return Crests.crestColors[crestColorID];
    }

    public void SetCrest()
    {
        crestSet = true;

        if (Crests.crestColorsSet == false)
        {
            Crests.SetHeraldColors();   
        }

        #region set champion crest pattern
        crestPatternID = Random.Range(0, Crests.crestPatternCount);
        #endregion

        #region setting champion crest color 
        // The goal is to make each color repeat as little as possible within one class

        // PEASANT
        if (champClass == 0)
        {
            // all colors have been used up - reset color rooster
            if (Crests.unusedPeasantColors.Count == 0)
            {
                Crests.ResetPeasantColors();
            }
            crestColorID = Random.Range(0, Crests.unusedPeasantColors.Count - 1);
            Crests.unusedPeasantColors.RemoveAt(crestColorID);
        }
        // KNIGHT
        else if (champClass == 1)
        {
            // all colors have been used up - reset color rooster
            if (Crests.unusedKnightColors.Count == 0)
            {
                Crests.ResetKnightColors();
            }
            crestColorID = Random.Range(0, Crests.unusedKnightColors.Count - 1);
            Crests.unusedKnightColors.RemoveAt(crestColorID);
        }
        // ARCHER
        else
        {
            // all colors have been used up - reset color rooster
            if (Crests.unusedArcherColors.Count == 0)
            {
                Crests.ResetArcherColors();
            }
            crestColorID = Random.Range(0, Crests.unusedArcherColors.Count - 1);
            Crests.unusedArcherColors.RemoveAt(crestColorID);
        }
        #endregion
    }

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

    public Attack.Type GetChampionAttackType()
    {
        if (champClass == 0)
        {
            return Attack.Type.Peasant;
        }
        else if (champClass == 1)
        {
            return Attack.Type.Knight;
        }
        else 
        {
            return Attack.Type.Archer;
        }
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

    public string GetFirstName()
    {
        string firstName = "";
        bool firstNameFound = false;

        foreach (char c in Name)
        {
            if (c == ' ')
            {
                firstNameFound = true;
                break;
            }
            else if (!firstNameFound)
                firstName = firstName + c;
        }
        return firstName;
    }
    
    // sir/lady    
    public string GetTitle()
    {
        if (isMan)
            return "Sir";
        else
            return "Lady";
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

