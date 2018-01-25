using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class SaveLoad : MonoBehaviour
{

    public static List<GameData> savedGames = new List<GameData>();
    
    public void Save()
    {
      //  Debug.Log(PlayerProfile.Singleton.SaltCurrent);
        // var p = PlayerProfile.Singleton;
        GameData.current = new GameData();
        GameData.current.salt = PlayerProfile.Singleton.SaltCurrent;
       // Debug.Log(GameData.current.salt);
        SaveLoad.savedGames.Add(GameData.current);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd"); //you can call it anything you want
        bf.Serialize(file, SaveLoad.savedGames);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            SaveLoad.savedGames = (List<GameData>)bf.Deserialize(file);
            Debug.Log(SaveLoad.savedGames[0].salt);

            PlayerProfile.Singleton.SaltCurrent = SaveLoad.savedGames[0].salt; // NOTE - DOES NOT UPDATE GameData!
            file.Close();

           // PlayerProfile.Singleton.SaltCurrent = 
        }
    }
}