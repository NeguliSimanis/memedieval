using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCastleController : MonoBehaviour
{
    private bool spawnEnemies = true;
    public bool spawnPeasants = true;
    public bool spawnArchers = true;
    public bool spawnKnights = true;

    public static Attack.Type CurrentEnemy; //= Attack.Type.Archer;
    [SerializeField] private Spawn EnemyPeasant;
    [SerializeField] private Spawn EnemyKnight;
    [SerializeField] private Spawn EnemyArcher;
    [SerializeField] private int SpawnCooldown;

    private float CountToStart; // delay between each enemy wave
    private float CountToSpawn;

    private void Start()
    {
        ChooseEnemyClass(); 
        CountToSpawn = SpawnCooldown;
        CountToStart = 3;
    }


    void Update()
    {
        if (CountToStart >= 0)
        {
            CountToStart -= Time.deltaTime;
        }

        else if (CountToSpawn >= 0 && spawnEnemies)
        {
            CountToSpawn -= Time.deltaTime;
            switch (CurrentEnemy)
            {
                case (Attack.Type.Archer):
                    EnemyArcher.SpawnCharacter();
                    return;
                case (Attack.Type.Knight):
                    EnemyKnight.SpawnCharacter();
                    return;
                case (Attack.Type.Peasant):
                    EnemyPeasant.SpawnCharacter();
                    return;
            }
            if (CurrentEnemy == Attack.Type.Archer) EnemyArcher.SpawnCharacter();
            if (CurrentEnemy == Attack.Type.Knight) EnemyKnight.SpawnCharacter();
            if (CurrentEnemy == Attack.Type.Peasant) EnemyPeasant.SpawnCharacter();
        }
        else
        {
            CountToSpawn = SpawnCooldown;
            CountToStart = 4;
            ChooseEnemyClass();
        }
    }

    private void ChooseEnemyClass()
    {
        // limitations to spawnable unit types exist
        if (!spawnPeasants || !spawnKnights || !spawnArchers)
        {
            LimitCurrentEnemies();
            Debug.Log("Limiting current enemies");
        }

        // no limitations on possible unit spawns - choose randomly
        else
        {
            var values = Attack.Type.GetValues(typeof(Attack.Type));
            CurrentEnemy = (Attack.Type)values.GetValue(Random.Range(0, values.Length));
        }
    }

    private void LimitCurrentEnemies()
    {
        // all classes are not allowed
        if (!SpawnPermissionCheck())
            return;

        // select random enemy class
        var values = Attack.Type.GetValues(typeof(Attack.Type));
        int currentEnemyID = Random.Range(0, values.Length);
        CurrentEnemy = (Attack.Type)values.GetValue(currentEnemyID);

        // two random classes is not allowed 
        if (!IsAllowedEnemyType(Attack.Type.Archer) && !IsAllowedEnemyType(Attack.Type.Peasant))
        {
            CurrentEnemy = Attack.Type.Knight;
        }
        else if (!IsAllowedEnemyType(Attack.Type.Archer) && !IsAllowedEnemyType(Attack.Type.Knight))
        {
            CurrentEnemy = Attack.Type.Peasant;
        }
        else if (!IsAllowedEnemyType(Attack.Type.Knight) && !IsAllowedEnemyType(Attack.Type.Peasant))
        {
            CurrentEnemy = Attack.Type.Peasant;
        }
        // one random class is not allowed
        else
        {
            currentEnemyID++;
            if (currentEnemyID == values.Length)
                currentEnemyID = 0;
        }
        CurrentEnemy = (Attack.Type)values.GetValue(currentEnemyID);
    }

    private bool SpawnPermissionCheck()
    {
        if (!spawnArchers && !spawnKnights && !spawnPeasants)
        {
            spawnEnemies = false;
            return false;
        }
        return true;
    }

    private bool IsAllowedEnemyType(Attack.Type attackType)
    {
    
        if (!spawnArchers && CurrentEnemy == attackType && attackType == Attack.Type.Archer)
        {
            //CurrentEnemy = Attack.Type.Peasant;
            return false;
        }
        else if (!spawnPeasants && CurrentEnemy == attackType && attackType == Attack.Type.Peasant)
        {
            //CurrentEnemy = Attack.Type.Knight;
            return false;
        }
        else if (!spawnKnights && CurrentEnemy == attackType && attackType == Attack.Type.Knight)
        {
            //CurrentEnemy = Attack.Type.Archer;
            return false;
        }
        return true;
    }
}

