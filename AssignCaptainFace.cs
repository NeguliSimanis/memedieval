using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignCaptainFace : MonoBehaviour
{

	public void AwakeChildren ()
    {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
	}
	

}
