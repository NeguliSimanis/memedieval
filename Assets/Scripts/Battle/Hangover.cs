using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hangover : MonoBehaviour {

    /*
     * Boosts player damage and decreases health in battle
     */

    Health battleHealth;
    [SerializeField]
    string battleHealthTag = "Player castle"; // will work if the same tag is set in the battle scene

    private float hangoverStartTime;
    private float hangoverDelay = 1f;
    private int hangoverDamage;
    public float hangoverBoost; // determined by Drink script
    private bool isHangover = false;
    private bool hasDrunk = false;

    public void InitiateHangover(int damageTaken, float attackBoost = 0f)
    {
        hasDrunk = true;
        isHangover = false;
        hangoverStartTime = Time.time + hangoverDelay;
        hangoverDamage = damageTaken;
        hangoverBoost = attackBoost;
        battleHealth = GameObject.FindGameObjectWithTag(battleHealthTag).GetComponent<Health>();
    }
    
    void Update ()
    {
        if (hasDrunk && !isHangover)
        {
            if (Time.time >= hangoverStartTime)
            {
                battleHealth.Damage(Attack.Type.defaultType, hangoverDamage);
                isHangover = true;
            }
        }
    }
}
