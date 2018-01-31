using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Champion : MonoBehaviour {

    public ChampionData properties;
    public bool onBattle;

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
    
    public void LevelUP()
    {
        properties.level++;
        properties.skillpoints++;
        //Debug.Log(Name + " LEVEL UP");
    }
}
