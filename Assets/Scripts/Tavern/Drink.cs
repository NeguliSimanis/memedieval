using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink : MonoBehaviour {

    #region variables
    [SerializeField]
    PlayerProfile playerProfile;

    [SerializeField]
    int drinkCost = 1;

    [SerializeField]
    int drinkHPDecrease = 10;

    [SerializeField]
    float drinkAttackIncrease = 100f;
    #endregion

    public void DrinkMead()
    {
        if (playerProfile.SpendDucats(drinkCost))
        {
            // Debug.Log("That was refreshing!");
            playerProfile.Drink(drinkHPDecrease, drinkAttackIncrease);      
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
