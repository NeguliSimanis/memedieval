using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitController : MonoBehaviour
{

    /*
    void OnApplicationQuit()
    {
        //Debug.Log("Application ending after " + Time.time + " seconds");
    }*/

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();

    }
}