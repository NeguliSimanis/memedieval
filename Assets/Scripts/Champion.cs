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
    
    /*public void LevelUP()
    {
        properties.level++;
        properties.skillpoints++;
        Debug.Log(properties.Name + " LEVEL UP");
    }*/

    public void EarnExp(int exp)
    {
        Debug.Log("earned " + exp + "exp");
        properties.currentExp += exp;
        
        while (properties.currentExp >= properties.nextLevelExp)
        {
            properties.currentExp -= properties.nextLevelExp;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        Debug.Log("level up");
        properties.level++;
        properties.skillpoints++;
        properties.nextLevelExp = Mathf.RoundToInt(properties.nextLevelExp * 1.5f);
    }

    public string GetClassName()
    {
        switch (properties.champClass)
        {
            case 0:
                return "Archer";
            case 1:
                return "Knight";
            case 2:
                return "Peasant";
        }
        return "error";
    }
}
