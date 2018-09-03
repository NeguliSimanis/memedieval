using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Market : MonoBehaviour
{
    #region variables
    [SerializeField]
    Text saltCostText;
    [SerializeField]
    Text saltAmountText;

    [Header("Warning texts")]
    [SerializeField] GameObject cantPurchaseSaltText;
    [SerializeField] GameObject cantPurchaseMeatText;

    [Header("Buy buttons")]
    [SerializeField] Button getSaltButton;
    [SerializeField] Button getMeatButton;

    [Header("Exchange rates")]
    [SerializeField] int saltGainPerPurchase = 8;
    public int saltPriceInDucats = 2;

    [SerializeField] int meatGainPerPurchase = 20;
    [SerializeField] int meatPriceInSalt = 4;
    // ducats earned from watching ad is in ShowAd.cs
    #endregion

    void Start()
    {
        saltCostText.text = saltPriceInDucats.ToString();
        saltAmountText.text = saltGainPerPurchase.ToString();

        getSaltButton.onClick.AddListener(GetSaltForDucats);
        getMeatButton.onClick.AddListener(GetMeatForSalt);
        CheckIfEnoughResources(Resources.Salt, Resources.Ducats, saltPriceInDucats);
    }

    /// <summary>
    /// Turns on/off the button for completing a transaction/watching ad (transactionButtonObject)
    /// </summary>
    /// <param name="enableTransaction">true = transaction should be enabled, false = transaction should be disabled </param>
    /// <param name="transactionButtonObject">the button for completing transaction/watching ad</param>
    /// <param name="transactionImpossibleText">the text that is displayed when transaction is impossible</param>
    private void EnableTransaction(bool enableTransaction, GameObject transactionButtonObject, GameObject transactionImpossibleText)
    {
        transactionButtonObject.SetActive(enableTransaction);
        transactionImpossibleText.SetActive(!enableTransaction);
    }

    public bool CheckIfEnoughResources(Resources offerType, Resources priceType, int priceAmount)
    {
        #region buying salt
        if (offerType == Resources.Salt && priceType == Resources.Ducats)
        {
            // enough resources
            if (PlayerProfile.Singleton.DucatCurrent >= priceAmount)
            {
                EnableTransaction(true, getSaltButton.gameObject, cantPurchaseSaltText);
                return true;
            }
        // not enough resources
        EnableTransaction(false, getSaltButton.gameObject,cantPurchaseSaltText);
        return false;
        }
        #endregion

        #region buying meat
        else if (offerType == Resources.Meat && priceType == Resources.Salt)
        {
            // enough resources
            if (PlayerProfile.Singleton.SaltCurrent >= priceAmount)
            {
                EnableTransaction(true, getMeatButton.gameObject, cantPurchaseMeatText);
                return true;
            }
        }
        // not enough resources
        EnableTransaction(false, getMeatButton.gameObject, cantPurchaseMeatText);
        return false;
        #endregion

    }

    public void GetSaltForDucats()
    {
        if (!CheckIfEnoughResources(Resources.Salt, Resources.Ducats, saltPriceInDucats))
            return;

        PlayerProfile.Singleton.SaltCurrent += saltGainPerPurchase;
        PlayerProfile.Singleton.DucatCurrent -= saltPriceInDucats;

        // disable salt purchase button if not enough resources after transaction
        CheckIfEnoughResources(Resources.Salt, Resources.Ducats, saltPriceInDucats);

        // enable purchasing meat if enough resources after transaction
        CheckIfEnoughResources(Resources.Meat, Resources.Salt, meatPriceInSalt);

    }

    public void GetMeatForSalt()
    {
        if (!CheckIfEnoughResources(Resources.Meat, Resources.Salt, meatPriceInSalt))
            return;

        PlayerProfile.Singleton.MeatCurrent += meatGainPerPurchase;
        PlayerProfile.Singleton.SaltCurrent -= meatPriceInSalt;

        // disable button if not enough resources after purchase
        CheckIfEnoughResources(Resources.Meat, Resources.Salt, meatPriceInSalt);
    }


}
