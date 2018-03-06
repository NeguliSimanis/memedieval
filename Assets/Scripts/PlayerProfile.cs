using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerProfile : MonoBehaviour
{
    public int profileID = 0;
    public int lastGameStatus;  //0-null -1-defeat 1-victory
    public string name = "Player";
    public List<Champion> champions = new List<Champion>();

    #region battle
    [SerializeField]
    string battleSceneName = "Test scene";

    //private int baseHealth = 100;      // goes down from drinking
    //private float attackModifier = 1f;   // goes up as you drink
    #endregion

    #region drinking 
    public bool isDrunk = false;
    private int drinkHPEffect = 0;
    private float drinkAttackEffect = 0f;
    Hangover hangover;
    #endregion

    #region resources
    private int saltCurrent = 10;
    private int ducatCurrent = 10;

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
    #endregion

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

    private void Start()
    {
        MictransactionsManager.Instance.OnTransactionSuccessful += Instance_OnTransactionSuccessful;
    }

    private void Instance_OnTransactionSuccessful(MictransactionsManager.PremiumCurrencyProduct product)
    {
        DucatCurrent += (int)product.CurrencyReceived;
    }

    public bool SpendDucats(int amount)
    {
        if (ducatCurrent < amount)
            return false;
        else
            ducatCurrent = ducatCurrent - amount;
        return true;
    }

    public void Drink(int hpDecrease, float attackIncrease)
    {
        isDrunk = true;
        drinkHPEffect = drinkHPEffect + hpDecrease;
        drinkAttackEffect = drinkAttackEffect + attackIncrease;
    }

    private void ModifyBattleProperties()
    {
        if (isDrunk)
        {
            hangover = gameObject.GetComponent<Hangover>();
            hangover.InitiateHangover(drinkHPEffect, drinkAttackEffect);
        } 
    }

    public void ResetBattleProperties()
    {
        isDrunk = false;
        drinkHPEffect = 0;
        drinkAttackEffect = 0f;
        if (hangover == null)
            hangover = gameObject.GetComponent<Hangover>();
        hangover.Reset();
    }

    #region listen to scene changes
    // dont really know how this stuff works, just copied from unity forums
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == battleSceneName)
        {
            ModifyBattleProperties();
        }

        profileID++;

        //destroy copies of this game object
        GameObject[] playerCopies;
        playerCopies = GameObject.FindGameObjectsWithTag(this.gameObject.tag);
        foreach (GameObject playerCopy in playerCopies)
        {
            if (playerCopy != this.gameObject)
            {
                int otherID = playerCopy.GetComponent<PlayerProfile>().profileID;
                if (otherID < profileID)
                    Destroy(playerCopy);
            }
        }
    }
    #endregion
}
