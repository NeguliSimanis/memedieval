using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    private static int totalEnemies;
    private static int totalAllies;
    private static int allyCounter;
    private static int enemyCounter;

    private Waypoint Target;
    private Attack attackClass;
    private PlayerUnit playerAttackClass;

    [SerializeField] private bool isEnemy;

    public float Speed;
    public bool isRallyingShoutBoostActive = false;
    public bool isDashing = false;

    [Header("Animations")]
    [SerializeField] private bool hasVictoryDance = false;
    [SerializeField]
    private SpriteRenderer victoryAnimation;

    [SerializeField]
    bool hasAttackAnimation;
    [SerializeField]
    SpriteRenderer attackAnimation;
    float attackAnimationDuration = 0.57f;
    float endAttackTime;
    bool isAttackCooldown = false;

    private void Awake()
    {
        ResetAllValues();
    }

    public void ChangeSpeed(float amount)
    {
        Debug.Log("speed before: " + Speed);
        Speed = Speed + amount;
        Debug.Log("new speed: " + Speed);
    }

    void Start()
    {

        if (isEnemy)
        {
            attackClass = gameObject.GetComponent<Attack>();
            enemyCounter++;
            totalEnemies++;
        }
        else
        {
            playerAttackClass = gameObject.GetComponent<PlayerUnit>();
            allyCounter++;
            totalAllies++;
        }
    }


    void Update()
    {
        if (Target == null || Health.Victory || Health.GameOver) return;

        // stop moving enemy unit if its attacking
        if (isEnemy && attackClass.TargetAmount() > 0)
        {
            PlayAttackAnimation(true);
            return;
        }

        // stop moving player unit if its attacking
        if (!isEnemy && playerAttackClass.TargetAmount() > 0)
        {
            PlayAttackAnimation(true);
            return;
        }

        // don't move unit if it's playing attack animation
        if (isAttackCooldown)
        {
            if (Time.time > endAttackTime)
            {
                isAttackCooldown = false;
                return;
            }
        }

        PlayAttackAnimation(false);
        transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, Target.transform.position) <= float.Epsilon)
        {
            if (isEnemy) Target = Target.Previous;
            else Target = Target.Next;
        }
    }


    private void OnDestroy()
    {
        if (isEnemy) enemyCounter--;
        else allyCounter--;
    }


    public void SetTarget(Waypoint Target)
    {
        this.Target = Target;
    }


    public static void ResetAllValues()
    {
        totalEnemies = 0;
        totalAllies = 0;
        allyCounter = 0;
        enemyCounter = 0;
    }

    // unit stops moving
    public void Stop()
    {
        Speed = 0f;
    }

    // unit stops and celebrates victory
    public void Celebrate()
    {
        Stop();
        if (hasVictoryDance)
        {
            // plays victory animation
            victoryAnimation.enabled = true;

            // hides walking animation
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void PlayAttackAnimation(bool isAttacking)
    {
        //Debug.Log("Attacking " + isAttacking);
        if (hasAttackAnimation)
        {
            if (isAttacking)
            {
                Debug.Log("attacking");
                // plays attack animation
                attackAnimation.enabled = true;

                // hides walking animation
                gameObject.GetComponent<SpriteRenderer>().enabled = false;

                // sets time when to stop animating
                endAttackTime = Time.time + attackAnimationDuration;
                isAttackCooldown = true;
            }
            else if (!isAttackCooldown)
            {
                // hides attack animation
                attackAnimation.enabled = false;

                // plays walking animation
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        
    }
}

