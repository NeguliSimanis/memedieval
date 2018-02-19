using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink : MonoBehaviour {

    [SerializeField]
    PlayerProfile playerProfile;

    [SerializeField]
    int drinkCost = 1;


    public void DrinkMead()
    {
        if (playerProfile.SpendDucats(drinkCost))
        {
            Debug.Log("That was refreshing!");
            /*
             * TO-DO:
             * > change tavern dialogue
             * > add sfx
             * > inform player about losing health? (nah)
             */
        }
        else
        {
            Debug.Log("Can't afford anymore drinks. You have a problem, man.");
            /*
             * TO-DO:
             * > change tavern dialogue
             * > add sfx
             */
        }
    }
}
