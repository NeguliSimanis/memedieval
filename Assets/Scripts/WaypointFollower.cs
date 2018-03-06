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


    private void Awake()
    {
        ResetAllValues();
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
        if (isEnemy && attackClass.TargetAmount() > 0) return;
        if (!isEnemy && playerAttackClass.TargetAmount() > 0) return;

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
}

