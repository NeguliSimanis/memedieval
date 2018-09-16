using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;

public class SoundSettings : MonoBehaviour {

    [SerializeField]
    private string musicVCAPath;
	[SerializeField]
	private string soundVCAPath;
	private float musicVolume;
	private float musicfinalVolume;
	private float soundVolume;
	private float soundFinalVolume;
    public bool soundMuted = false;


    private FMOD.Studio.VCA musicVCA;
	private FMOD.Studio.VCA soundVCA;
    // Use this for initialization
    void Start () {

        musicVCA = RuntimeManager.GetVCA(musicVCAPath);
		musicVCA.getVolume(out musicVolume, out musicfinalVolume);
		soundVCA = RuntimeManager.GetVCA(soundVCAPath);
		soundVCA.getVolume(out soundVolume, out soundFinalVolume);

        //UnityEngine.Debug.Log("volume " + volume + " - final volume:" + finalVolume);
		
	}
	
    public void MuteMusic()
    {
        CreateGameData.CreateIfNoGameDataExists();
		if (soundMuted)
        {
            musicVCA.setVolume(1f);
			soundVCA.setVolume(1f);
            UnityEngine.Debug.Log("mute not called");
            soundMuted = false;
            GameData.current.soundMuted = false;
        }
        else
        {
            musicVCA.setVolume(0f);
			soundVCA.setVolume(0f);
            UnityEngine.Debug.Log("mute called");
            soundMuted = true;
            GameData.current.soundMuted = true;
        } 
    }
}
