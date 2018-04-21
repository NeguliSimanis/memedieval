using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChampionAbilities : MonoBehaviour {

    Champion champion;

    WaypointFollower waypointFollower;
    float defaultMoveSpeed;

    Animator animator;
    float defaultAnimSpeed;

    bool hasChargingBar = false;
    bool canChargeAbility = true;
    bool isChargingAbility = false;
    bool isWaitingOrder = false;
    bool isOnCooldown = false;
    bool isAbilityActive = false;

    private float chargingDuration = 3f; 

    private float abilityEffectDuration = 3.5f;
    private float abilityAvailableDuration = 2f;
    private float abilityCooldown = 5f;

    private float chargingStartTime;
    private float chargingEndTime;
    private float cooldownEndTime;
    private float waitingEndTime;
    private float abilityEffectEndTime;

    [SerializeField]
    GameObject chargingBar;
    Image chargingBarImage;
    Transform canvasTransform;

    [SerializeField]
    GameObject abilityReadyAnimation;

    [SerializeField]
    private AudioClip abilitySFX;
    private Image abilityImage;
    ChampionData.Ability ability;

    // components necessary for berserk fury
    private PlayerUnit championUnit;
    float furyAttackSpeedEffect = 2f;
    float furyMoveSpeedEffect = 3f;
    float defaultAttackCooldown;

    #region praying
    private int healAmount = 20;
    #endregion

    void Start()
    {
        SetDefaultValues();
        GetComponents();
        FindAbilityEffect();
    }

    void GetComponents()
    {
        canvasTransform = gameObject.transform.Find("Canvas").GetComponent<Canvas>().transform;
        chargingBarImage = chargingBar.transform.Find("AbilityChargingBar").gameObject.GetComponent<Image>();
        champion = gameObject.GetComponent<Champion>();
        ability = champion.properties.currentChampionAbility;
        championUnit = gameObject.GetComponent<PlayerUnit>();
        //abilitySFX = gameObject.GetComponent<Champion>().properties.championAbilitySFX;
    }

    void FindAbilityEffect()
    { 
        if (ability == ChampionData.Ability.BerserkFury)
        {
            abilityImage = GameObject.Find("BerserkFury").GetComponent<Image>();
        }
        else if (ability == ChampionData.Ability.Prayer)
        {
            abilityImage = GameObject.Find("Prayer").GetComponent<Image>();
        }
        else // (ability == ChampionData.Ability.RallyingShout)
        {
            abilityImage = GameObject.Find("RallyingShout").GetComponent<Image>();
        }
    }

    void SetDefaultValues()
    {
        waypointFollower = gameObject.GetComponent<WaypointFollower>();
        defaultMoveSpeed = waypointFollower.Speed;

        animator = gameObject.GetComponent<Animator>();
        defaultAnimSpeed = animator.speed;
    }

	void OnMouseDown()
    {
        if (canChargeAbility)
        {
            ChargeAbility();
        }
        else if (isWaitingOrder)
        {
            UseAbility(); 
        }
    }

    void UseAbility()
    {
        //Debug.Log("using abilty!");
        abilityImage.enabled = true;
        isWaitingOrder = false;
        isOnCooldown = true;
        
        abilityReadyAnimation.SetActive(false);
        cooldownEndTime = Time.time + abilityCooldown;
        abilityEffectEndTime = Time.time + abilityEffectDuration;
       
        if (ability == ChampionData.Ability.BerserkFury)
        {
            StartBerserkFury();
        }
        else if (ability == ChampionData.Ability.Prayer)
        {
            StartPrayer();
        }
        else if (ability == ChampionData.Ability.RallyingShout)
        {
            StartRallyingShout();
        }
        isAbilityActive = true;
        // gameObject.GetComponent<AudioSource>().PlayOneShot(abilitySFX, 0.7F);
    }

    void ChargeAbility()
    {
        chargingBar.gameObject.SetActive(true);
        
        canChargeAbility = false;
        isChargingAbility = true;

        chargingStartTime = Time.time;
        chargingEndTime = chargingStartTime + chargingDuration;

        waypointFollower.Speed = 0;
        animator.speed = 0;
    }

    void EndCharging()
    {
        isChargingAbility = false;
        chargingBar.gameObject.SetActive(false);
        WaitOrder();
    }

    void UpdateChargingBarImage()
    {
        chargingBarImage.fillAmount = (Time.time - chargingStartTime)/chargingDuration;
    }

    // waits when the player will use the ability
    void WaitOrder()
    {
        isWaitingOrder = true;
        abilityReadyAnimation.SetActive(true);
        waitingEndTime = Time.time + abilityAvailableDuration;
    }

    void StopWaitingOrder()
    {
        isWaitingOrder = false;
        abilityReadyAnimation.SetActive(false);
    }

    void Reset(bool allowCharging = true)
    {
        if (isWaitingOrder)
            StopWaitingOrder();
        if (isAbilityActive)
            StopUsingAbility();
        abilityImage.enabled = false;
        canChargeAbility = allowCharging;
        waypointFollower.Speed = defaultMoveSpeed;
        animator.speed = defaultAnimSpeed;
    }

    void StopUsingAbility()
    {
        if (ability == ChampionData.Ability.BerserkFury)
        {
            EndBerserkFury();
        }
        else if (ability == ChampionData.Ability.Prayer)
        {
            EndPrayer();
        }
        else if (ability == ChampionData.Ability.RallyingShout)
        {
            EndRallyingShout();
        }
        isAbilityActive = false;
    }

    void Update()
    {
        if (isChargingAbility)
        {

            UpdateChargingBarImage();
            if (Time.time > chargingEndTime)
            {
                EndCharging();
            }
        }

        else if (isWaitingOrder)
        {
            if (Time.time > waitingEndTime)
                Reset();
        }

        else if (isAbilityActive)
        {
            if (Time.time > abilityEffectEndTime)
            {
                isAbilityActive = false;
                Reset(false);
            }        
        }
        else if (isOnCooldown)
        {
            if (Time.time > cooldownEndTime)
                Reset();
        }
    }

    void StartBerserkFury()
    {
        Debug.Log("Berserk fury activated");
        defaultAttackCooldown = championUnit.defaultCooldown;
        if (champion.GetClassName() == "Archer")
            championUnit.defaultCooldown = 0.4f;//championUnit.cooldown / furyAttackSpeedEffect;
        else
            championUnit.defaultCooldown = 0.2f;
        animator.speed = 3f; //animator.speed * furyMoveSpeedEffect;
        waypointFollower.Speed = 3f; // waypointFollower.Speed * furyMoveSpeedEffect;
    }

    void EndBerserkFury()
    {
        Debug.Log("Berserk fury deactivated");
        championUnit.defaultCooldown = defaultAttackCooldown;
        animator.speed = defaultAnimSpeed;
        waypointFollower.Speed = defaultMoveSpeed;

    }

    void StartPrayer()
    {
        Debug.Log("Prayer activated");
        PlayerUnit[] playerUnits;
        playerUnits = Object.FindObjectsOfType<PlayerUnit>();
        foreach (PlayerUnit playerUnit in playerUnits)
        {
            playerUnit.gameObject.GetComponent<Health>().Regen(healAmount, abilityEffectDuration);
        }
        animator.speed = defaultAnimSpeed;
        waypointFollower.Speed = defaultMoveSpeed;
    }

    void EndPrayer()
    {
        Debug.Log("Prayer deactivated");
    }

    void StartRallyingShout()
    {
        Debug.Log("RallyingShout activated");
    }

    void EndRallyingShout()
    {
        Debug.Log("RallyingShout deactivated");
    }

}
