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

    public bool armySelectedInStrategyView = false;
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
    private int saltCurrent = 5;
    private int ducatCurrent = 10;
    private int meatCurrent = 100;

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

    public int MeatCurrent
    {
        get { return meatCurrent; }
        set { meatCurrent = value; }
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
        if (DisableScreenSleep.current == null)
            DisableScreenSleep.current = new DisableScreenSleep();
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

    public void ModifyBattleProperties()
    {
        if (isDrunk)
        {
            hangover = gameObject.GetComponent<Hangover>();
            hangover.InitiateHangover(drinkHPEffect, drinkAttackEffect);
        }

        ChampionEffect championEffect = this.gameObject.GetComponent<ChampionEffect>();
        championEffect.SetTotalEffect();
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
        DestroyPlayerProfileCopies();
        // disable screen sleeping while the game is open 
        if (DisableScreenSleep.current == null)
            DisableScreenSleep.current = new DisableScreenSleep();
        DisableScreenSleep.current.DisableSleep();
        // resets a health script variable
        Health.endBattleCalled = false;
        if (scene.name == battleSceneName)
            InitializeBattleScene();
    }

    void DestroyPlayerProfileCopies()
    {
        profileID++;
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

    // starts battle without displaying map if just exited strategy view
    void InitializeBattleScene()
    {
        if (armySelectedInStrategyView)
        {
            armySelectedInStrategyView = false;

            // TODO: replace finding game object with singleton?
            GameObject.Find("Map").GetComponent<Map>().EnterBattle();
        }
    }

    #endregion
}
