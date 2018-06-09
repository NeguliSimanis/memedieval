using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Champion : MonoBehaviour {

    public ChampionData properties;
    public bool onBattle;
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
    
        properties.skillpoints++;
        properties.nextLevelExp = Mathf.RoundToInt(properties.nextLevelExp * 1.5f);
    }

    // TO-DO - REMOVE
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
}
