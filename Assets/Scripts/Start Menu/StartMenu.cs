using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Upon opening the game, check if save file exists.
/// If yes, load it directly and go to map scene.
/// 
/// Remembers whether the saved game had muted sound
/// 
/// 15.09.2018. Sīmanis Mikoss
/// </summary>

public class StartMenu : MonoBehaviour
{
    #region variables
    [SerializeField]
    SoundSettings soundSettings;

    [Header("Script settings")]
    [SerializeField]
    bool loadGameDataAutomatically = true; // if false, cannot remember whether sfx was muted
    [SerializeField]
    bool loadNextLVAutomatically = true;
    bool rememberSFXSettings = true;
    string levelToLoad = "Test scene";

    // for testing if mute works after delay
    float muteDelay = 1f;
    float muteStartTime;
    bool muteActive = false;
    #endregion

    void Start ()
    {
        muteStartTime = Time.time + muteDelay;
        if (!loadGameDataAutomatically)
            return;
        LoadGameDataAutomatically();
        LoadNextLevel();
    }

    private void Update()
    {
        if (!muteActive)
        {
            muteActive = true;
            RememberSFXSettings();
        }
    }

    void LoadGameDataAutomatically()
    {
        SaveLoad saveLoad = gameObject.GetComponent<SaveLoad>();

        // check if save file exists
        if (saveLoad.CheckSaveFile())
        {
            // save exists - load data
            saveLoad.Load();
        }
    }
	/// <summary>
    /// Mutes the SFX if they were muted in the save file
    /// </summary>
    void RememberSFXSettings()
    {
        CreateGameData.CreateIfNoGameDataExists();
        if (GameData.current.soundMuted)
            soundSettings.MuteMusic();
    }

    void LoadNextLevel()
    {
        if (loadNextLVAutomatically)
            gameObject.GetComponent<LoadScene>().loadLevel(levelToLoad);
    }
}

