using Sacristan.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MictransactionsManager : Singleton<MictransactionsManager>
{
    public delegate void TransactionHandler(PremiumCurrencyProduct product);
    public event TransactionHandler OnTransactionSuccessful;
    public event TransactionHandler OnTransactionCanceled;

    [System.Serializable]
    public class FakeTransactionUI
    {
        [SerializeField]
        private GameObject root;

        [SerializeField]
        private Text descriptionText;

        [SerializeField]
        private Text priceText;

        public void Enable()
        {
            root.SetActive(true);
        }

        public void Disable()
        {
            root.SetActive(false);
        }

        public void UpdateUI(PremiumCurrencyProduct product)
        {
            descriptionText.text = product.Description;
            priceText.text = product.PriceInEuro;
        }
    }

    public class PremiumCurrencyProduct
    {
        private readonly string description;
        private readonly float priceInEuro;
        private readonly uint currencyReceived;

        public PremiumCurrencyProduct(string description, float priceInEuro, uint currencyReceived)
        {
            this.description = description;
            this.priceInEuro = priceInEuro;
            this.currencyReceived = currencyReceived;
        }

        public string Description { get { return description; } }
        public string PriceInEuro { get { return priceInEuro.ToString("n2") + "€"; } }
        public uint CurrencyReceived { get { return currencyReceived; } }
    }

    public static readonly PremiumCurrencyProduct PremiumCurrencyProduct_Ducats10 = new PremiumCurrencyProduct("10 Ducats (Premium Currency)", 0.99f, 10);
    public static readonly PremiumCurrencyProduct PremiumCurrencyProduct_Ducats120 = new PremiumCurrencyProduct("120 Ducats (Premium Currency)", 9.99f, 120);

    [SerializeField]
    private FakeTransactionUI fakeTransactionUI;

    private PremiumCurrencyProduct lastCurrencyProductChosen;

    public void InitialisePayment(PremiumCurrencyProduct currencyProduct)
    {
        lastCurrencyProductChosen = currencyProduct;

        fakeTransactionUI.UpdateUI(currencyProduct);
        fakeTransactionUI.Enable();
    }

    public void PaymentDone()
    {
        Debug.Log("PaymentDone");

        fakeTransactionUI.Disable();
        if (OnTransactionSuccessful != null) OnTransactionSuccessful.Invoke(lastCurrencyProductChosen);
    }

    public void PaymentCanceled()
    {
        Debug.Log("PaymentCanceled");
        fakeTransactionUI.Disable();
        if (OnTransactionCanceled != null) OnTransactionCanceled.Invoke(lastCurrencyProductChosen);
    }

}
