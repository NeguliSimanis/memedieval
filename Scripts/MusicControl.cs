using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour {




	private Health enemyCastleHealth;
	private FMODUnity.StudioEventEmitter music;



	void Start () 
	{

		music = this.GetComponent<FMODUnity.StudioEventEmitter> ();

		music.SetParameter ("Transition", 0f);

		//DontDestroyOnLoad("Music")
	}
	
	// Update is called once per frame
	void Update () {


		if (GameObject.FindGameObjectWithTag ("EnemyCastle").activeInHierarchy) 
		{

			enemyCastleHealth = GameObject.FindGameObjectWithTag ("EnemyCastle").GetComponent<Health> ();
			Debug.Log (enemyCastleHealth.CurrentHealth);


			if (enemyCastleHealth.CurrentHealth > 350 && enemyCastleHealth.CurrentHealth < 499)
				music.SetParameter ("Transition", 1f);
			if (enemyCastleHealth.CurrentHealth > 150 && enemyCastleHealth.CurrentHealth < 349)
				music.SetParameter ("Transition", 2f);
			if (enemyCastleHealth.CurrentHealth > 0 && enemyCastleHealth.CurrentHealth < 149)
				music.SetParameter ("Transition", 3f);
				
		
		}
			
	
	}


}
