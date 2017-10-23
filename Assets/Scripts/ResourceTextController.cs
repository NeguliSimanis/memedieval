using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceTextController : MonoBehaviour
{
    [SerializeField] private Text resourcesText;
    [SerializeField] private Resources playerResources;
    [SerializeField] private Resources enemyResources;

    void Start()
    {
        resourcesText.text = "Meat: " + playerResources.Amount;
    }


    void Update()
    {
        resourcesText.text = "Meat: " + playerResources.Amount;
    }


    public void AddResources(bool enemy, int amount)
    {
        //Debug.Log(enemy + ", " + amount);
        if (enemy) enemyResources.Amount = amount;
        else playerResources.Amount = amount;
    }
}

