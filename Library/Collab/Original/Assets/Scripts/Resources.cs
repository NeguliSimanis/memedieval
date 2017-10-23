using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.

public class Resources : MonoBehaviour {
    [SerializeField] private int resources;
    [SerializeField] private int resourcesPerFiveSeconds;

    // variables for player resources bar
    [SerializeField] Image resourceBar;
    [SerializeField] Text resourceText;

    private float second;

    public bool WasEnoughResources(int resources)
    {
        if (this.resources >= resources) {
            this.resources -= resources;
            return true;
        }
        return false;
    }

    void Start()
    {
        second = 0;
    }

    void Update()
    {
        if (resourceText != null)
        {
            //update player resource text
            resourceText.text = resources.ToString();

            //update player resource bar
            resourceBar.fillAmount = resources * 0.001f;
        }

        second += Time.deltaTime;
        if (second >= 5)
        {
            second -= 5;
            resources += resourcesPerFiveSeconds;
            //Debug.Log("Added resources.");
        }
    }
}
