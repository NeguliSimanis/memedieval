﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    private static bool victory;
    private static bool gameOver;
    private static bool PeasantCaptainDead;
    private static bool ArcherCaptainDead;
    private static bool KnightCaptainDead;

    [SerializeField] private Attack.Type UnitType;
    [SerializeField] private int meadCarrying;
    [SerializeField] private bool IsCharacter;
    [SerializeField] private bool isPlayer;
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject fire;
    [SerializeField] private bool isCaptain;


    [SerializeField] LoadScene nextLevelScript;
    [SerializeField] string nextLevelToLoad;

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
        //Debug.Log("current salt: " + GameData.current.salt);
        currentHealth = MaximumHealth;
    }


    void Update()
    {

        if (gameObject.tag.Equals("EnemyCastle") && fire != null && !fire.activeSelf && currentHealth <= MaximumHealth / 3f)
        {
            fire.SetActive(true);
        } //else if (gameObject.tag.Equals("EnemyCastle") && fire != null && fire.activeSelf && CurrentHealth > MaximumHealth / 3f)

        if (victory || gameOver)
        {
            PlayerProfile.Singleton.ResetBattleProperties();
            if (victory)
            {
                PlayerProfile.Singleton.lastGameStatus = 1;
                var c = PlayerProfile.Singleton.champions;
                for (int i = 0; i < c.Count; i++)
                {
                    if (c[i].onBattle == true)
                    {
                        c[i].LevelUP();
                        c[i].onBattle = false;
                    }
                }
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
                SceneManager.LoadScene("Castle");
            }

        }
        
        if (currentHealth <= 0)
        {
            Debug.Log("battle over");
            Debug.Log(gameObject.tag);
            if (isCaptain)
            {
                //Debug.Log("Captain died");
                if (UnitType == Attack.Type.Archer) ArcherCaptainDead = true;
                if (UnitType == Attack.Type.Peasant) PeasantCaptainDead = true;
                if (UnitType == Attack.Type.Knight) KnightCaptainDead = true;
            }
            if (IsCharacter) {
                FindObjectOfType<ResourceTextController>().AddResources(isPlayer, meadCarrying);
                GameObject.Destroy(gameObject);
            }
            else
            {
                if (isPlayer)
                {
                    Debug.Log("player loses");
                    gameOver = true;
                    victory = false;
                }
                else if (!isPlayer) victory = true;
            }
        }
    }


    public static bool GameOver
    {
        get { return gameOver; }
    }


    public static bool Victory
    {
        get { return victory; }
    }


    public void Damage(Attack.Type attackingType, int damageAmount)
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
            currentHealth = MaximumHealth - damageAmount;
            if (!IsCharacter && healthBar != null)
                healthBar.fillAmount = (currentHealth * 1f) / MaximumHealth;
        }
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
}
