using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// script makes an UI image follow a given game object
public class FollowObject : MonoBehaviour {

    public GameObject objectToFollow;
    bool isFollowing = false;

    public void StartFollowing (GameObject target)
    {

        objectToFollow = target;
        isFollowing = true;
    }

	void Update ()
    {
        if (!isFollowing)
            return;
        //var objectPos = Camera.main.WorldToScreenPoint(objectToFollow.transform.position);
        Debug.Log("following " + objectToFollow.name);
        gameObject.transform.position = objectToFollow.transform.position;
        //gameObject.transform.position = objectPos;
        
	}
}
