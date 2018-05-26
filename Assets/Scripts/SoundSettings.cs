using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;

public class SoundSettings : MonoBehaviour {

    [SerializeField]
    private string musicVCAPath;

    public float volume;
    public float finalVolume;
    public bool musicMuted = true;


    private FMOD.Studio.VCA musicVCA;
    // Use this for initialization
    void Start () {

        musicVCA = RuntimeManager.GetVCA(musicVCAPath);
        musicVCA.getVolume(out volume, out finalVolume);


        //UnityEngine.Debug.Log("volume " + volume + " - final volume:" + finalVolume);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MuteMusic()
    {
        if (musicMuted)
        {
            musicVCA.setVolume(0f);
            musicMuted = false;
        }

        else
        {
            musicVCA.setVolume(1f);
            musicMuted = true;
        }
            
    }

}
