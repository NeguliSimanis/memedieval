using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Champion : MonoBehaviour {

    public ChampionData properties;
    public bool onBattle;
    /// <summary>
    /// shows whether the champion currently has an active button game object for summoning units in battle
    /// </summary>
    public bool hasUnitButton = false;
    public bool invitedToBattle = false;

    /* This all is now stored in GameData script in ChampionData class
      public Texture2D picture;
      public string Name;
      public bool isMan;
      public Texture2D picture;
      public int level;
      public int skillpoints=3;
      public int champClass;

      public string Bio;
      public string quote;
      public bool isDead;
      */

    public Champion(ChampionData newProperties, bool onBattle = false)
    {
        properties = newProperties;
    }
    
    // NOTE - does not remove champion from neutral champions list!
    public void DeleteChampion()
    {
        List<Champion> playerChampions = PlayerProfile.Singleton.champions;
        for (int i = 0; i < playerChampions.Count; i++)
        {
            if (playerChampions[i] == this)
                playerChampions.Remove(playerChampions[i]);
        }
        
        Destroy(this.gameObject);
    }

    public void EarnExp(int exp)
    {
        properties.currentExp += exp;
        while (properties.currentExp >= properties.nextLevelExp)
        {
            properties.currentExp -= properties.nextLevelExp;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        properties.level++;
        properties.skillpoints++;
        properties.nextLevelExp = Mathf.RoundToInt(properties.nextLevelExp * 1.5f);
    }

    // TODO - REMOVE
    //This is a carbon copy of a similar ChampionData function
    public string GetClassName()
    {
        switch (properties.champClass)
        {
            case 2:
                return "Archer";    
            case 1:
                return "Knight";
            case 0:
                return "Peasant";
        }
        return "error";
    }

    #region listen to scene changes to reset variables
    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        hasUnitButton = false;
    }
    #endregion
}
