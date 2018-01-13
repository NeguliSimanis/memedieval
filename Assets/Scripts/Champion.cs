using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Champion : MonoBehaviour {
    public string Name;
    public bool isMan;
    public Texture2D picture;
    public int level;
    public int skillpoints=3;
    public int champClass;
    public bool onBattle;
    public string Bio;
    public string quote;
    public bool isDead;
    public void LevelUP()
    {
        level++;
        skillpoints++;
        Debug.Log(Name + " LEVEL UP");
    }
}
