using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// script makes an UI image follow a given game object
public class FollowObject : MonoBehaviour {

    public GameObject objectToFollow;
    //private Image thisImage;

	// Use this for initialization
	void Start ()
    {
        //thisImage = gameObject.GetComponent<Image>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
        var objectPos = Camera.main.WorldToScreenPoint(objectToFollow.transform.position);
        gameObject.transform.position = objectPos;
        
	}
}
