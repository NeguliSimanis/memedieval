using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour {

    int dashEffect = 2;

    bool isCooldown = false;
    float dashDuration = 2f;
    float dashCooldown = 3f;
    float dashAvailableTime = 0f;
    float dashResetTime;

    float defaultMoveSpeed;
    float defaultAnimSpeed;

    Animator animator;
    WaypointFollower waypointFollower;

    void Start()
    {
        waypointFollower = gameObject.GetComponent<WaypointFollower>();
        animator = gameObject.GetComponent<Animator>();
        defaultMoveSpeed = waypointFollower.Speed;
        defaultAnimSpeed = animator.speed;
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
            
            waypointFollower.Speed *= dashEffect;
            animator.speed *= dashEffect;
        }
    }

    void Update()
    {
        if (Time.time > dashResetTime)
        {
            waypointFollower.Speed = defaultMoveSpeed;
            animator.speed = defaultAnimSpeed;
        }
    }
}
