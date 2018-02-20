using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerProfile : MonoBehaviour
{

    public int lastGameStatus;  //0-null -1-defeat 1-victory
    public string name = "Player";
    public List<Champion> champions = new List<Champion>();

    #region battle
    [SerializeField]
    string battleSceneName = "Main";

    Health battleHealth;
    [SerializeField]
    string battleHealthTag = "Player castle"; // will work if the same tag is set in the battle scene

    //private int baseHealth = 100;      // goes down from drinking
    private float attackModifier = 1f;   // goes up as you drink
    #endregion

    #region drinking 
    private bool isDrunk = false;
    private int drinkHPEffect = 0;
    private float drinkAttackEffect = 0f;
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
        if (!isDrunk)
            isDrunk = true;
        drinkHPEffect = drinkHPEffect + hpDecrease;
        drinkAttackEffect = drinkAttackEffect + attackIncrease;

        //Debug.Log("Hp decrease:" + drinkHPEffect);
        //Debug.Log("Attack increase:" + drinkAttackEffect);
    }

    private void ModifyBattleProperties()
    {
        battleHealth = GameObject.FindGameObjectWithTag(battleHealthTag).GetComponent<Health>();

        if (isDrunk)
        {
            Debug.Log("drink hp decrease: " + drinkHPEffect);
            battleHealth.Damage(Attack.Type.defaultType, drinkHPEffect);
        } 
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
    }
    #endregion
}
