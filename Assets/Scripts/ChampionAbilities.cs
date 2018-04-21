using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChampionAbilities : MonoBehaviour {

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

    private float abilityEffectDuration = 4f;
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
    [SerializeField]
    private Image abilityImage;
    ChampionData.Ability ability;
    void Start()
    {
        FindAbilityEffect();
        SetDefaultValues();
        GetComponents();      
    }

    void GetComponents()
    {
        canvasTransform = gameObject.transform.Find("Canvas").GetComponent<Canvas>().transform;
        chargingBarImage = chargingBar.transform.Find("AbilityChargingBar").gameObject.GetComponent<Image>();
        ability = gameObject.GetComponent<Champion>().properties.currentChampionAbility;
        //abilitySFX = gameObject.GetComponent<Champion>().properties.championAbilitySFX;
    }

    void FindAbilityEffect()
    { 
        GameObject abilityImageObject;
        if (ability == ChampionData.Ability.BerserkFury)
        {
            abilityImageObject = GameObject.Find("BerserkFury");
        }
        else if (ability == ChampionData.Ability.Prayer)
        {
            abilityImageObject = GameObject.Find("Prayer");
        }
        else // (ability == ChampionData.Ability.RallyingShout)
        {
            abilityImageObject = GameObject.Find("RallyingShout");
        }
        abilityImage = abilityImageObject.GetComponent<Image>();
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
        isAbilityActive = true;

        abilityReadyAnimation.SetActive(false);
        cooldownEndTime = Time.time + abilityCooldown;
        abilityEffectEndTime = Time.time + abilityEffectDuration;
        
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
        abilityImage.enabled = false;
        canChargeAbility = allowCharging;
        waypointFollower.Speed = defaultMoveSpeed;
        animator.speed = defaultAnimSpeed;
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
    }

    void EndBerserkFury()
    {
        Debug.Log("Berserk fury deactivated");
    }

    void StartPrayer()
    {
        Debug.Log("Prayer activated");
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
