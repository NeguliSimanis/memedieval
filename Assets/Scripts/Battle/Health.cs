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

    private static bool victory;
    private static bool gameOver;
    private static bool PeasantCaptainDead;
    private static bool ArcherCaptainDead;
    private static bool KnightCaptainDead;



    [SerializeField] private Attack.Type UnitType;
    [SerializeField] private int meadCarrying;
    [SerializeField] private bool IsCharacter;
    [SerializeField] private bool isPlayer; // as opposed to player unit
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject fire;
    [SerializeField] private bool isCaptain;


    [SerializeField] LoadScene nextLevelScript;
    [SerializeField] string nextLevelToLoad = "Castle";
    [SerializeField] string enemyBalancerTag = "Enemy balancer";


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
        if (isCelebratingVictory && Input.GetMouseButtonDown(0))
        {
            if (!isLoadingNextLevel)
                StartCoroutine(LoadNextLevelAfterDelay());
        }

        if (isRegening && Time.time > regenEndTime)
        {
            EndRegen();
        }

        if (fire != null && !fire.activeSelf && currentHealth <= MaximumHealth / 3f)
        {
            fire.SetActive(true);
        } //else if (gameObject.tag.Equals("EnemyCastle") && fire != null && fire.activeSelf && CurrentHealth > MaximumHealth / 3f)

        if (victory || gameOver)
        {
            PlayerProfile.Singleton.ResetBattleProperties();
            if (victory)
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
            if (nextLevelScript == null) Debug.Log("LoadScene script not specified in inspector");
            if (nextLevelScript == null) Debug.Log("Next level name not specified in inspector");
            else// nextLevelScript.loadLevel(nextLevelToLoad); 
            {
                Spawn.ResetAllValues();
                WaypointFollower.ResetAllValues();
                ResetAllValues();
                if (victory)
                    CelebrateVictory();
                else
                    MournDefeat();
                
            }

        }
        
        if (currentHealth <= 0)
        {
            if (isCaptain)
            {
                //Debug.Log("Captain died");
                if (UnitType == Attack.Type.Archer) ArcherCaptainDead = true;
                if (UnitType == Attack.Type.Peasant) PeasantCaptainDead = true;
                if (UnitType == Attack.Type.Knight) KnightCaptainDead = true;
            }
            if (IsCharacter) {
                FindObjectOfType<ResourceTextController>().AddResources(isPlayer, meadCarrying);

                DestroyPlayerUnit();
                //GameObject.Destroy(gameObject);
            }
            else
            {
                if (isPlayer)
                {
                    MournDefeat();
                    gameOver = true;
                    victory = false;
                }
                else if (!isPlayer) WinBattle(); 
            }
        }
    }

    void WinBattle()
    {
        victory = true;
        AllocateExp();
        
        if (GameData.current == null)
        {
            GameData.current = new GameData();
        }

        // mark destroyed castle
        int defeatedCastleID = GameObject.FindGameObjectWithTag(enemyBalancerTag).GetComponent<EnemyBalancer>().currentCastleID;
        GameData.current.destroyedCastles[defeatedCastleID] = true;

        PlayerProfile.Singleton.gameObject.GetComponent<ChampionEffect>().ResetChampionEffect();
    }

    private void AllocateExp()
    {
        ExpController expController = GameObject.FindGameObjectWithTag("BattleController").GetComponent<ExpController>();

        expController.AllocateChampionExp();

        /*ChampionEffect championEffect =  PlayerProfile.Singleton.gameObject.GetComponent<ChampionEffect>();

        foreach (Champion champion in PlayerProfile.Singleton.champions)
        {
            //Debug.Log("checking champion");
            if (champion.invitedToBattle == true)
            {
                //Debug.Log(champion.properties.Name + " deserves exp");
                champion.EarnExp(championEffect.championFinalExpEarn);
                champion.onBattle = false;
            }
        }*/
    }

    public static bool GameOver
    {
        get { return gameOver; }
    }


    public static bool Victory
    {
        get { return victory; }
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
        Debug.Log("regen ended");
    }

    public static bool Archer
    {
        get { return ArcherCaptainDead; }
    }


    public static bool Knight
    {
        get { return KnightCaptainDead; }
    }


    public static bool Peasant
    {
        get { return PeasantCaptainDead; }
    }

    public void ResetAllValues()
    {
        victory = false;
        gameOver = false;
        PeasantCaptainDead = false;
        ArcherCaptainDead = false;
        KnightCaptainDead = false;
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
        if (!isLoadingNextLevel)
            StartCoroutine(LoadNextLevelAfterDelay());
    }

    IEnumerator LoadNextLevelAfterDelay(float delay = 3f)
    {  
        isLoadingNextLevel = true;
        Debug.Log("loading next level");
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nextLevelToLoad);
    }

}

