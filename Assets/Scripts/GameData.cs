using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The script stores all data that has to be saved/loaded 
 */

[System.Serializable]
public class GameData {
    public static GameData current;
    public int salt = 0;
    public int ducats = 0; 
}
