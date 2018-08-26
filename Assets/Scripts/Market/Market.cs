using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Market : MonoBehaviour
{
    #region variables
    [SerializeField] Button getSaltButton;
    [SerializeField] GameObject notEnoughResourcesText;

    [SerializeField]
    Text saltCostText;
    [SerializeField]
    Text saltAmountText;

    [Header("Exchange rates")]
    [SerializeField] int saltGainPerPurchase = 8;
    [SerializeField] int saltPriceInDucats = 2;
    #endregion

    void Start()
    {
        saltCostText.text = saltPriceInDucats.ToString();
        saltAmountText.text = saltGainPerPurchase.ToString();

        getSaltButton.onClick.AddListener(GetSaltForDucats);
        CheckIfEnoughResources(Resources.Salt, Resources.Ducats, saltPriceInDucats);
    }


    private void EnableButton(bool enableButton, GameObject buttonObject)
    {
        buttonObject.SetActive(enableButton);
        notEnoughResourcesText.SetActive(!enableButton);
    }

    private bool CheckIfEnoughResources(Resources offerType, Resources priceType, int priceAmount)
    {
        // buying salt by paying ducats
        if (offerType == Resources.Salt && priceType == Resources.Ducats)
        {
            // enough resources
            if (PlayerProfile.Singleton.DucatCurrent >= priceAmount)
                return true;
        }
        // not enough resources
        EnableButton(false, getSaltButton.gameObject);
        return false;
    }

    public void GetSaltForDucats()
    {
        if (!CheckIfEnoughResources(Resources.Salt, Resources.Ducats, saltPriceInDucats))
            return;

        PlayerProfile.Singleton.SaltCurrent += saltGainPerPurchase;
        PlayerProfile.Singleton.DucatCurrent -= saltPriceInDucats;

        // disable button if not enough resources after purchase
        CheckIfEnoughResources(Resources.Salt, Resources.Ducats, saltPriceInDucats);

    }

    public void GetMeatForSalt()
    {

    }


}
