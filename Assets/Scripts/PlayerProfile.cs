using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerProfile : MonoBehaviour
{

    private static PlayerProfile _player;
    public static PlayerProfile Singleton
    {
        get
        {
            if (_player != null)
                return _player;
            _player = FindObjectOfType<PlayerProfile>();
            if (_player != null)
                return _player;
            var g = new GameObject();
            var p = g.AddComponent<PlayerProfile>();
            var d = g.AddComponent<DontDestroyOnLoad>();
            d.keepChildren = true;
            _player = p;
            return _player;
        }
    }
    public int lastGameStatus;//0-nekas -1-zaudējums 1-uzvara
    public string name = "Player";
    //to do or not do
    public List<Champion> champions = new List<Champion>();

    private int saltCurrent = 10;
    private int ducatCurrent = 10;
    private int baseHealth = 100;

    public int SaltCurrent
    {
        get { return saltCurrent; }
        set { saltCurrent = value; }
    }

    public int DucatCurrent
    {
        get { return ducatCurrent; }
        set { ducatCurrent = value; }
    }

    private void Start()
    {
        MictransactionsManager.Instance.OnTransactionSuccessful += Instance_OnTransactionSuccessful;
    }

    private void Instance_OnTransactionSuccessful(MictransactionsManager.PremiumCurrencyProduct product)
    {
        DucatCurrent += (int)product.CurrencyReceived;
    }
}
