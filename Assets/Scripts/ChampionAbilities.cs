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

    private float chargingDuration = 3f; 

    private float abilityEffectDuration = 4f;
    private float abilityAvailableDuration = 2f;
    private float abilityCooldown = 5f;

    private float chargingStartTime;
    private float chargingEndsTime;

    private float waitingEndTime;

    [SerializeField]
    GameObject chargingBar;
    Image chargingBarImage;
    Transform canvasTransform;

    [SerializeField]
    GameObject abilityReadyAnimation;

    void Start()
    {
        SetDefaultValues();
        canvasTransform = gameObject.transform.Find("Canvas").GetComponent<Canvas>().transform;
        chargingBarImage = chargingBar.transform.Find("AbilityChargingBar").gameObject.GetComponent<Image>();
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
    }

    void ChargeAbility()
    {
        chargingBar.gameObject.SetActive(true);
        
        canChargeAbility = false;
        isChargingAbility = true;

        chargingStartTime = Time.time;
        chargingEndsTime = chargingStartTime + chargingDuration;

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

    void Reset()
    {
        if (isWaitingOrder)
            StopWaitingOrder();   
        canChargeAbility = true;
        waypointFollower.Speed = defaultMoveSpeed;
        animator.speed = defaultAnimSpeed;
    }

    void Update()
    {
        if (isChargingAbility)
        {

            UpdateChargingBarImage();
            if (Time.time > chargingEndsTime)
            {
                EndCharging();
            }
        }

        if (isWaitingOrder)
        {
            if (Time.time > waitingEndTime)
                Reset();
        }
    }


}
