using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaymentButton : MonoBehaviour
{

    [SerializeField] bool paymentsAvailable = false;
    public int purchaseAmount;

    [SerializeField] GameObject errorMessage;

    // payment buttons
    [SerializeField] GameObject paymentButton1;
    [SerializeField] GameObject paymentButton2;
    [SerializeField] GameObject exitPaymentsButton;

    // dialogue variables
    Text npcText;
    Text replyText;
    [SerializeField] string npcText1 = "Psst! Hey!\nFancy earning a coin?";
    [SerializeField] string npcText2 = "Here's {0} Ducats.\nPleasure doing business.";
    [SerializeField] string replyText1 = "Tell me more...";
    [SerializeField] string replyText2 = "I'm not done with you...";

    // objects that have attached text 
    [SerializeField] GameObject showPayments; // Tell me more...
    [SerializeField] GameObject exitButton;   // Begone, heathen!
    [SerializeField] GameObject npcDialoguePanel;     // Psst! Hey! Fancy earning a coin?

    Text _npcText;
    Text _replyText;

    #region Properties

    private Text NPCText
    {
        get
        {
            if (_npcText == null) _npcText = npcDialoguePanel.GetComponentInChildren<Text>();
            return _npcText;
        }
    }

    private Text ReplyText
    {
        get
        {
            if (_replyText == null) _replyText = showPayments.GetComponentInChildren<Text>();
            return _replyText;
        }
    }
    #endregion

    void Start()
    {
        _npcText = npcDialoguePanel.GetComponentInChildren<Text>();
        _replyText = showPayments.GetComponentInChildren<Text>();

        MictransactionsManager.Instance.OnTransactionSuccessful += MictransactionsManager_OnTransactionSuccessful;
        MictransactionsManager.Instance.OnTransactionCanceled += Instance_OnTransactionCanceled;

        ResetNpcText();
    }

    public void InitiatePayment(int option)
    {
        HidePaymentButtons();

        switch (option)
        {
            case 0:
                MictransactionsManager.Instance.InitialisePayment(MictransactionsManager.PremiumCurrencyProduct_Ducats10);
                break;
            case 1:
                MictransactionsManager.Instance.InitialisePayment(MictransactionsManager.PremiumCurrencyProduct_Ducats120);
                break;
        }
    }

    void HidePaymentButtons()
    {
        paymentButton1.SetActive(false);
        paymentButton2.SetActive(false);
        exitPaymentsButton.SetActive(false);
    }

    void ChangeNpcText()
    {
        NPCText.text = string.Format(npcText2, purchaseAmount);
        ReplyText.text = replyText2;
    }

    void ShowTransactionDialogue()
    {
        ChangeNpcText();
        showPayments.SetActive(true);
        exitButton.SetActive(true);
    }

    public void ResetNpcText()
    {
        NPCText.text = npcText1;
        ReplyText.text = replyText1;
    }

    #region Event callbacks
    private void MictransactionsManager_OnTransactionSuccessful(MictransactionsManager.PremiumCurrencyProduct product)
    {
        purchaseAmount = (int) product.CurrencyReceived;
        ShowTransactionDialogue();
    }

    private void Instance_OnTransactionCanceled(MictransactionsManager.PremiumCurrencyProduct product)
    {
        HidePaymentButtons();
        ResetNpcText();
        showPayments.SetActive(true);
        exitButton.SetActive(true);
    }

    #endregion
}
