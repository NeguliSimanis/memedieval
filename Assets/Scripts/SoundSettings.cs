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
    public bool soundMuted = true;


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
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MuteMusic()
    {
		if (soundMuted)
        {
            musicVCA.setVolume(0f);
			soundVCA.setVolume(0f);
			soundMuted = false;
        }

        else
        {
            musicVCA.setVolume(1f);
			soundVCA.setVolume(0f);
			soundMuted = true;
        }
            
    }

}
