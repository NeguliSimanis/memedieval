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

    [SerializeField] private bool Enemy;
    public float Speed;


    private void Awake()
    {
        ResetAllValues();
    }


    void Start()
    {
        attackClass = gameObject.GetComponent<Attack>();
        if (Enemy)
        {
            enemyCounter++;
            totalEnemies++;
        }
        else
        {
            allyCounter++;
            totalAllies++;
        }
    }


    void Update()
    {
        if (Target == null || Health.Victory || Health.GameOver || attackClass.TargetAmount() > 0) return;

        transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, Target.transform.position) <= float.Epsilon)
        {
            if (Enemy) Target = Target.Previous;
            else Target = Target.Next;
        }
    }


    private void OnDestroy()
    {
        if (Enemy) enemyCounter--;
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

