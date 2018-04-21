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

    private float chargingDuration = 3f; 

    private float abilityEffectDuration = 4f;
    private float abilityAvailableDuration = 2f;
    private float abilityCooldown = 5f;

    private float chargingStartTime;
    private float chargingEndsTime;

    [SerializeField]
    GameObject chargingBar;
    Image chargingBarImage;
    Transform canvasTransform;

    void Start()
    {
        SetDefaultValues();
        canvasTransform = gameObject.transform.Find("Canvas").GetComponent<Canvas>().transform;
        chargingBarImage = chargingBar.GetComponent<Image>();
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
        chargingBar.SetActive(true);
        
        canChargeAbility = false;
        isChargingAbility = true;

        chargingStartTime = Time.time;
        chargingEndsTime = chargingStartTime + chargingDuration;

        waypointFollower.Speed = 0;
        animator.speed = 0;
    }

    void EndCharging()
    {
        canChargeAbility = true;
        isChargingAbility = false;
        waypointFollower.Speed = defaultMoveSpeed;
        animator.speed = defaultAnimSpeed;
        chargingBar.SetActive(false);
    }

    void UpdateChargingBarImage()
    {
        chargingBarImage.fillAmount = (Time.time - chargingStartTime)/chargingDuration;
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
    }


}
