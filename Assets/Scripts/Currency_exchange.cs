using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency_exchange : MonoBehaviour {



	// Use this for initialization
	void Start () {
		if(GameObject.Find("PlayerProfile").GetComponent<PlayerProfile>().DucatCurrent >= 10){
					GameObject.Find("PlayerProfile").GetComponent<PlayerProfile>().DucatCurrent -= 10;
					GameObject.Find("PlayerProfile").GetComponent<PlayerProfile>().SaltCurrent += 50;
				}
		}





	//	GameObject PlayerDucats = GameObject.Find("PlayerProfile");
	//	PlayerProfile playerProfile = PlayerDucats.GetComponent<PlayerProfile>();
	//	if(playerProfile.DucatCurrent >= 10) {
	//		playerProfile.DucatCurrent -= 10;
	//		playerProfile.SaltCurrent += 50;
	//	}


	// Update is called once per frame
	void Update () {
		
	}

}
