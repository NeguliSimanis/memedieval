using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TavernDialogueControl : MonoBehaviour {

    /*
     * TO-DO:
     * move all inkeeper and shady guy dialogue logic to this script
     */

    PlayerProfile playerProfile;
    private string playerProfileTag = "Player profile";

    #region Inkeeper
    private Drink drink;
    private int switchDrinkText = 0;

    #region Inkeeper dialogue
    [Header("Inkeeper dialogue")]
    [SerializeField]
    Text inkeeperText;

    [SerializeField]
    Button drinkMeadButton;

    [SerializeField]
    private string inkeeperDefaultText = "How may I serve you, Sire?";

    [SerializeField]
    private string drinkSold1 = "The finest brew of the Dutchy! How else may I serve?";

    [SerializeField]
    private string drinkSold2 = "Another one?";

    [SerializeField]
    private string drinkSold3 = "Sire has an unquenchable thirst";

    [SerializeField]
    private string cantAfffordDrink = "My humble apologies, Sire. There's no more mead.";
    #endregion
    #endregion

    void Start()
    {
        playerProfile = GameObject.FindGameObjectWithTag(playerProfileTag).GetComponent<PlayerProfile>();
        drink = drinkMeadButton.GetComponent<Drink>();
        drinkMeadButton.onClick.AddListener(DrinkMead);
    }

    void DrinkMead()
    {
        // can afford drink
        if (drink.DrinkMead() == true)
        {
            if (switchDrinkText == 0)
            {
                inkeeperText.text = drinkSold1;
                switchDrinkText = 1;
            }
            else if (switchDrinkText == 1)
            {
                inkeeperText.text = drinkSold2;
                switchDrinkText = 2;
            }
            else
            {
                inkeeperText.text = drinkSold3;
                switchDrinkText = 0;
            }
        }
        // can't afford drink
        else
        {
            inkeeperText.text = cantAfffordDrink;
        }      
    }
}
