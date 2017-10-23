using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCastleController : MonoBehaviour
{
    public static Attack.Type CurrentEnemy = Attack.Type.Archer;
    [SerializeField] private Spawn EnemyPeasant;
    [SerializeField] private Spawn EnemyKnight;
    [SerializeField] private Spawn EnemyArcher;
    [SerializeField] private int RoundTime;
    [SerializeField] private Text WarningText;

    private float CountToStart;
    private float CountToSpawn;
    private bool TextSet;

    private void Start()
    {
        CountToSpawn = RoundTime;
        CountToStart = 3;
    }


    void Update()
    {
        if (CountToStart < 3 && !TextSet)
        {
            TextSet = true;
            switch (CurrentEnemy)
            {
                case Attack.Type.Archer:
                    WarningText.text = HardcodedText[0];
                    return;
                case Attack.Type.Knight:
                    WarningText.text = HardcodedText[1];
                    return;
                case Attack.Type.Peasant:
                    WarningText.text = HardcodedText[2];
                    return;
            }
        }

        if (CountToStart >= 0)
        {
            CountToStart -= Time.deltaTime;
        }

        else if (CountToSpawn >= 0)
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
            CountToSpawn = RoundTime;
            CountToStart = 4;

            var values = Attack.Type.GetValues(typeof(Attack.Type));
            CurrentEnemy = (Attack.Type) values.GetValue(Random.Range(0, values.Length));
        }
    }

    private string[] HardcodedText = {
        "The foe calls forth bowmen! Hold fast to thy shields!",
        "Steel thine bellies! Fiendish knights art upon us!",
        "Fetch thy arrows, men! Filthy peasants inbound!"
    };
}

