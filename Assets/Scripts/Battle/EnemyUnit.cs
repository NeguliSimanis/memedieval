using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{

    public int MaximumHealth;
    [SerializeField] private Attack.Type UnitType;
    [SerializeField] private int killReward;

    private EnemyBalancer enemyBalancer;

    private int currentHealth;

    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
    }

    void Start()    
    {
        enemyBalancer = GameObject.FindGameObjectWithTag(GameData.current.enemyBalancerTag).GetComponent<EnemyBalancer>();
        currentHealth = Mathf.RoundToInt(MaximumHealth * enemyBalancer.enemyUnitHPMultiplier);
        //currentHealth = MaximumHealth;
    }

    public void Damage(PlayerUnit.Type attackingType, int damageAmount)
    {
        Debug.Log("taking" + damageAmount + "damage");

        if (attackingType == PlayerUnit.Type.Knight && UnitType == Attack.Type.Archer)
            damageAmount *= 2;
        if (attackingType == PlayerUnit.Type.Archer)
        {
            if (UnitType == Attack.Type.Peasant)
                damageAmount = (int)(damageAmount * 1.5f);
        }

        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            FindObjectOfType<ResourceTextController>().AddResources(false, killReward);
            GameObject.Destroy(gameObject);
        }
    }

}

