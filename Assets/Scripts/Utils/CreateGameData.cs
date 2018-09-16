using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGameData
{
    /// <summary>
    /// Creates a new GameData.current if it does not exist already
    /// </summary>
    public static void CreateIfNoGameDataExists()
    {
        if (GameData.current == null)
        {
            GameData.current = new GameData();
        }
    }
}
