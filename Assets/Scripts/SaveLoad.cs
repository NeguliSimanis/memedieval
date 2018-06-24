using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/*
 * Saves/loads current game data as a binary file.
 * Script is attached to save/load buttons
 * 
 * References:
 *  > https://gamedevelopment.tutsplus.com/tutorials/how-to-save-and-load-your-players-progress-in-unity--cms-20934
 *  > https://github.com/NeguliSimanis/ExampleGame
 */

public class SaveLoad : MonoBehaviour
{
    public static List<GameData> savedGames = new List<GameData>();
    CreateChampion createChampion;

    private void Start()
    {
        createChampion = GameObject.FindGameObjectWithTag("ChampionCreator").GetComponent<CreateChampion>();
    }

    // saves current game data locally as a binary file
    public void Save()
    {
        // creates current GameData object
        if (GameData.current == null)
        {
            GameData.current = new GameData();
        }

        // stores the values of PlayerProfile in the current GameData object
        GameData.current.salt = PlayerProfile.Singleton.SaltCurrent;
        GameData.current.ducats = PlayerProfile.Singleton.DucatCurrent;
        int championCount = PlayerProfile.Singleton.champions.Count;

        GameData.current.championList.Clear();
        for (int i = 0; i < championCount; i++)
        {
            GameData.current.championList.Add(PlayerProfile.Singleton.champions[i].properties);
        }

        SaveLoad.savedGames.Clear();
        SaveLoad.savedGames.Add(GameData.current);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        bf.Serialize(file, SaveLoad.savedGames);
        file.Close();
    }

    // loads current game data from a local file
    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            SaveLoad.savedGames = (List<GameData>)bf.Deserialize(file);

            // set player resources
            PlayerProfile.Singleton.SaltCurrent = SaveLoad.savedGames[0].salt;          
            PlayerProfile.Singleton.DucatCurrent = SaveLoad.savedGames[0].ducats;

            // load game data
            GameData.current = new GameData();
            GameData.current.LoadGameProgress(SaveLoad.savedGames[0]);

            ClearChampionData();

            // load new champion data
            foreach (ChampionData championData in SaveLoad.savedGames[0].championList)
            {
                createChampion.LoadChampionFromSave(championData);
            }

            file.Close();
        }

        else
        {
            Debug.Log("No save file exists!");
        }
    }

    void ClearChampionData()
    {
        foreach (Champion champion in PlayerProfile.Singleton.champions)
        {
            Destroy(champion.gameObject);
        }
        PlayerProfile.Singleton.champions.Clear();
    }
}