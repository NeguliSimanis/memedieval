using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCastleController : MonoBehaviour
{
    // controls enemy spawning from the castle

    public bool spawnEnemies = true;

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
        var values = Attack.Type.GetValues(typeof(Attack.Type));
        CurrentEnemy = (Attack.Type)values.GetValue(Random.Range(0, values.Length));
        
    } 

    public void SpawnTutorialKnight()
    {
        EnemyKnight.SpawnCharacter();
    }

    public void SpawnTutorialPeasant()
    {
        EnemyPeasant.SpawnCharacter();
    }



}

