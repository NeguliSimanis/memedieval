using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    private bool isCelebratingVictory = false;
    private bool isMourningDefeat = false;
    private bool isLoadingNextLevel = false;
    private bool isRegening = false;
    private float regenEndTime;
    private int regenPerSecond;

    public static bool endBattleCalled = false; // to avoid calling the method multiple times
    private static bool isVictory;
    private static bool isDefeat;
    private static bool peasantChampionDead;
    private static bool archerChampionDead;
    private static bool knightChampionDead;

    [SerializeField] private Attack.Type UnitType;
    [SerializeField] private int meadCarrying;
    [SerializeField] private bool IsCharacter;
    [SerializeField] private bool isPlayer; // as opposed to player unit
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject fire;
    [SerializeField] private bool isCaptain;

    [SerializeField] LoadScene nextLevelScript;
    [SerializeField] string nextLevelToLoad = "Castle";

	private int currentHealth;
	public int CurrentHealth
	{
		get
		{
			return currentHealth;
		}
	}

    public int MaximumHealth;

    private void Awake()
    {
        ResetAllValues();
    }

    [Header("Battle UI")]
    [SerializeField]
    GameObject surrenderButton;
    public static bool canSurrender = false;
    public static bool surrenderButtonEnabled = false;

    void Start()
    {
        if (GameData.current == null)
            GameData.current = new GameData();

        // set health modifiers to player units
        if (!isPlayer && gameObject.tag != GameData.current.enemyCastleTag)
        {
            ChampionEffect championEffect = PlayerProfile.Singleton.gameObject.GetComponent<ChampionEffect>();
            currentHealth = Mathf.RoundToInt(MaximumHealth * championEffect.playerUnitHPCoefficient);
           // Debug.Log(UnitType + " hp is " + currentHealth);
        }
        else
            currentHealth = MaximumHealth;
    }

    void Update()
    {
        if (isRegening && Time.time > regenEndTime)
        {
            EndRegen();
        }

        if (fire != null && !fire.activeSelf && currentHealth <= MaximumHealth / 3f)
        {
            fire.SetActive(true);
        } //else if (gameObject.tag.Equals("EnemyCastle") && fire != null && fire.activeSelf && CurrentHealth > MaximumHealth / 3f)

        if (isVictory || isDefeat)
        {
            PlayerProfile.Singleton.ResetBattleProperties();
            if (isVictory)
            {
                PlayerProfile.Singleton.lastGameStatus = 1;
            }
            else
            {
                PlayerProfile.Singleton.lastGameStatus = -1;
                var c = PlayerProfile.Singleton.champions;
                for (int i = 0; i < c.Count; i++)
                {
                    if (c[i].onBattle == true)
                    {
                        c[i].properties.isDead = true;
                        c[i].onBattle = false;
                    }
                }
            }
            if (nextLevelScript == null) Debug.Log("Next level name not specified in inspector");
            else
            {
                Spawn.ResetAllValues();
                WaypointFollower.ResetAllValues();
                ResetAllValues();
                if (isVictory)
                    CelebrateVictory();
                else
                    MournDefeat();
            }
        }
        if (currentHealth <= 0)
        {
            if (isCaptain)
            {
                if (UnitType == Attack.Type.Archer)
                {
                    archerChampionDead = true;
                    Spawn.ArcherCaptainsLeft--;
                }
                if (UnitType == Attack.Type.Peasant)
                {
                    Spawn.PeasantCaptainsLeft--;
                    peasantChampionDead = true;
                }
                if (UnitType == Attack.Type.Knight)
                {
                    knightChampionDead = true;
                    Spawn.KnightCaptainsLeft--;
                }
                // all champions dead, enable surrender button
                if (Spawn.PeasantCaptainsLeft <= 0 && Spawn.KnightCaptainsLeft <= 0 && Spawn.ArcherCaptainsLeft <= 0)
                {
                    canSurrender = true;
                }
            }
            if (IsCharacter) {
                FindObjectOfType<ResourceTextController>().AddResources(isPlayer, meadCarrying);
                DestroyPlayerUnit();
            }
            else
            {
                if (isPlayer)
                {
                    MournDefeat();
                    isDefeat = true;
                    isVictory = false;
                }
                else if (!isPlayer)
                {
                    CelebrateVictory();
                    isVictory = true;
                    isDefeat = false;
                }
                EndBattle(isVictory);
            }
        }
        ManageSurrenderButton();
    }

    private void ManageSurrenderButton()
    {
        if (surrenderButton == null)
            return;
        if (!canSurrender)
            return;
        if (surrenderButtonEnabled)
            return;
        
        surrenderButtonEnabled = true;
        surrenderButton.SetActive(true);
    }

    public void EndBattle(bool isVictory = false)
    {
        if (endBattleCalled)
            return;
        endBattleCalled = true;

        // pause game
        PlayerProfile.Singleton.gameObject.transform.Find("TimeControl").GetComponent<TimeControl>().Pause();

        // victory is set here again for cases when function is called from outside health script
        Health.isVictory = isVictory; 

        BattleOver.manager = new BattleOver();
        BattleOver.manager.EndBattle(isVictory);
    }


    private void AllocateExp()
    {
        ExpController expController = GameObject.FindGameObjectWithTag("BattleController").GetComponent<ExpController>();

        expController.AllocateChampionExp();
    }

    public static bool GameOver
    {
        get { return isDefeat; }
    }


    public static bool Victory
    {
        get { return isVictory; }
    }

    void DestroyPlayerUnit()
    {
        gameObject.GetComponent<PlayerUnit>().Die();
    }

    public void Damage(int damageAmount, Attack.Type attackingType = Attack.Type.defaultType)
    {
        if (IsCharacter)
        {
            if (attackingType == Attack.Type.Knight && UnitType == Attack.Type.Archer)
                damageAmount *= 2;
            if (attackingType == Attack.Type.Archer)
            {
                if (UnitType == Attack.Type.Peasant)
                    damageAmount = (int)(damageAmount * 1.5f);
                //if (UnitType == Attack.Type.Knight)
                //    DamageAmount = (int)(DamageAmount * 0.75f);
            }
        }

        if (attackingType != Attack.Type.defaultType)
        {
            currentHealth -= damageAmount;
            if (!IsCharacter && healthBar != null)
                healthBar.fillAmount = (currentHealth * 1f) / MaximumHealth;
        }
        else
        {
            currentHealth -= damageAmount;
            if (!IsCharacter && healthBar != null)
                healthBar.fillAmount = (currentHealth * 1f) / MaximumHealth;
        }
    }

    public void StartRegen (int amount, float targetTime)
    {   
        regenEndTime = targetTime;
        Debug.Log(amount);
        regenPerSecond = (int)(amount/(regenEndTime-Time.time));
        isRegening = true;
        StartCoroutine(Regen());
        gameObject.transform.Find("RegenAnimation").gameObject.SetActive(true);
    }

    IEnumerator Regen()
    {
        Heal(regenPerSecond);
        Debug.Log(regenPerSecond);
        yield return new WaitForSeconds(1);
    }

    private void Heal (int amount)
    {
        currentHealth = currentHealth + amount;
    }

    private void EndRegen ()
    {
        StopCoroutine(Regen());
        isRegening = false;
        gameObject.transform.Find("RegenAnimation").gameObject.SetActive(false);
    }

    public static bool Archer
    {
        get { return archerChampionDead; }
    }

    public static bool Knight
    {
        get { return knightChampionDead; }
    }

    public static bool Peasant
    {
        get { return peasantChampionDead; }
    }

    public void ResetAllValues()
    {
        isVictory = false;
        isDefeat = false;
        peasantChampionDead = false;
        archerChampionDead = false;
        knightChampionDead = false;
    }

    public void SetMaxHealth(int value)
    {
        MaximumHealth = value;
        currentHealth = value;
    }

    private void CelebrateVictory()
    {
        if (!isCelebratingVictory)
        {
            isCelebratingVictory = true;
            var playerUnits = FindObjectsOfType<PlayerUnit>();
            foreach (PlayerUnit playerUnit in playerUnits)
            {
                playerUnit.gameObject.GetComponent<WaypointFollower>().Celebrate();
            }
        }
    }

    private void MournDefeat()
    {
        isMourningDefeat = true;
    }

}

