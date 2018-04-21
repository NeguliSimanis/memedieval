using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour {

    float dashEffect = 1.5f;

    bool isCooldown = false;
    float dashDuration = 2f;
    float dashCooldown = 3f;
    float dashAvailableTime = 0f;
    float dashResetTime;

    float defaultMoveSpeed;

    AnimatorSpeed animatorSpeed;
    WaypointFollower waypointFollower;

    void Start()
    {
        waypointFollower = gameObject.GetComponent<WaypointFollower>();
        animatorSpeed = gameObject.GetComponent<AnimatorSpeed>();
        defaultMoveSpeed = waypointFollower.Speed;
    }

	void OnMouseDown()
    {
        if (Time.time > dashAvailableTime)
        {
            isCooldown = false;
        }

        if (!isCooldown)
        {
            dashAvailableTime = Time.time + dashCooldown;
            dashResetTime = Time.time + dashDuration;
            
            waypointFollower.ChangeSpeed(dashEffect);
            waypointFollower.isDashing = true;
            animatorSpeed.ChangeAnimSpeed(dashEffect);
        }
    }

    void Update()
    {
        if (Time.time > dashResetTime)
        {
            if (waypointFollower.isDashing == true)
            {
                waypointFollower.isDashing = false;
                animatorSpeed.ChangeAnimSpeed(-dashEffect);
                waypointFollower.ChangeSpeed(-dashEffect);
            }
        }
    }
}
