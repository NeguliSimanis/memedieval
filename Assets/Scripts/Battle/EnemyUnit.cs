using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{

    public int MaximumHealth;
    [SerializeField] private Attack.Type UnitType;
    [SerializeField] private int killReward;
    [SerializeField] GameObject deathAnimation;

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
        // no modifiers to enemy ,ax health active (e.g. in tutorial)
        if (GameObject.FindGameObjectWithTag(GameData.current.enemyBalancerTag) == null)
        {
            return;
        }

        // modifiers to max health exist
        enemyBalancer = GameObject.FindGameObjectWithTag(GameData.current.enemyBalancerTag).GetComponent<EnemyBalancer>();
        currentHealth = Mathf.RoundToInt(MaximumHealth * enemyBalancer.enemyUnitHPMultiplier);
    }

    // damages this unit
    public void Damage(PlayerUnit.Type attackingType, int damageAmount)
    {
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
            Die();
        }
    }

    private void Die()
    {
        float deathLocationOffsetX;
        if (UnitType == Attack.Type.Archer)
        {
            deathLocationOffsetX = 0f;
        }
        else if (UnitType == Attack.Type.Knight)
        {
            deathLocationOffsetX = -0.3f;
        }
        else
        {
            deathLocationOffsetX = 0f;
        }
        Vector3 deathLocation = new Vector3(gameObject.transform.localPosition.x + deathLocationOffsetX, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
        GameObject deathObjectParent = Instantiate(new GameObject(), deathLocation, Quaternion.identity);
        GameObject deathObject = Instantiate(deathAnimation, deathObjectParent.transform);
        // deathObject.transform.localPosition = gameObject.transform.localPosition;
        deathObject.SetActive(true);

        Destroy(gameObject);
    }

}

