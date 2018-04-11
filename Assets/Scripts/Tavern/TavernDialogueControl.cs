using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TavernDialogueControl : MonoBehaviour {

    /*
     * TO-DO:
     * move all inkeeper and shady guy dialogue logic to this script
     */


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

    [SerializeField]
    private string noChampions = "You have no champions, Sire.";

    [SerializeField]
    private string tooManyChampions = "There are no swords for hire, Sire.";

    [SerializeField]
    private string notEnoughSalt = "More salt is required, Sire.";
    #endregion
    #endregion

    void Start()
    {
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

    public void SayNoChampions()
    {
        inkeeperText.text = noChampions;
    }

    public void SayTooManyChampions()
    {
        inkeeperText.text = tooManyChampions;
    }

    public void ResetDialogue()
    {
        inkeeperText.text = inkeeperDefaultText;
    }

    public void SayNotEnoughSalt()
    {
        inkeeperText.text = notEnoughSalt; 
    }
}
