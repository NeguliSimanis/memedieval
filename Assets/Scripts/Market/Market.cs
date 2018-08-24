using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Market : MonoBehaviour
{

    #region variables
    [SerializeField] Button getSaltButton;

    int saltGainPerDucats = 4;
    int saltPriceInDucats = 1;
    #endregion



    void Start()
    {
        getSaltButton.onClick.AddListener(GetSaltForDucats);
    }

    public void GetSaltForDucats()
    {
        // not enough ducats for transaction
        if (PlayerProfile.Singleton.DucatCurrent < saltPriceInDucats)
        {
            return;
        }
        // enough ducats for transaction
        PlayerProfile.Singleton.SaltCurrent += saltGainPerDucats;
        PlayerProfile.Singleton.DucatCurrent -= saltPriceInDucats;
    }

    public void GetMeatForSalt()
    {

    }


}
