using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChampionAbilities : MonoBehaviour {

    WaypointFollower waypointFollower;
    float defaultMoveSpeed;

    Animator animator;
    float defaultAnimSpeed;

    bool canChargeAbility = true;
    bool isChargingAbility = false;

    private float chargingDuration = 3f;
    private float abilityEffectDuration = 4f;
    private float abilityAvailableDuration = 2f;
    private float abilityCooldown = 5f;

    private float chargingEndsTime;

    [SerializeField]
    Texture2D chargingBar;
    private int chargingBarWidth = 100;
    private int chargingBarHeight = 20;
    int chargingCounter = 0;
    private Color chargingColor = new Color(0.875f, 0.445f, 0.167f, 1.000f);

    void Start()
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
        canChargeAbility = false;
        isChargingAbility = true;

        chargingEndsTime = Time.time + chargingDuration;

        waypointFollower.Speed = 0;
        animator.speed = 0;
    }

    void Update()
    {
        if (isChargingAbility)
        {
            //chargingBar.FillAreaByWidth(10, 10, chargingColor,10);
            chargingBar.FloodFillArea(1, 1, chargingColor, chargingCounter);
            chargingBar.Apply();
            chargingCounter++;

            if (Time.time > chargingEndsTime)
            {
                canChargeAbility = true;
                isChargingAbility = false;
                waypointFollower.Speed = defaultMoveSpeed;
                animator.speed = defaultAnimSpeed;
                chargingCounter = 0;
            }
        }
    }

    void OnGUI()
    {
        if (!isChargingAbility)
            return;

        Vector2 targetPos;
        targetPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        //chargingBar.FillAreaByWidth(10, 10, chargingColor, 10);
        
        //GUI.Box(new Rect(targetPos.x-100, targetPos.y, 100, 100), chargingBar);

        //chargingBar.FloodFillArea(10, 10, Color.red);
        //chargingBar.Apply();

        GUI.DrawTexture(new Rect(targetPos.x - 100, targetPos.y, chargingBarWidth, chargingBarHeight), chargingBar);

    }
}
