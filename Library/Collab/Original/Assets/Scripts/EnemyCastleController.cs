using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCastleController : MonoBehaviour
{
    Health castleHealth;
    [SerializeField]
    int castleDamage = 100; // TEMPORARY: how much damage will castle take from each unit
                           // will be updated to depend on the unit type

    // variables for spawning enemies
    [SerializeField] GameObject enemyKnight;
    [SerializeField] GameObject spawnPosition;      // where enemy units will spawn
    [SerializeField] Waypoint targetPosition;       // where enemy units will go (waypoint)
    [SerializeField] float minSpawnInterval = 2f;   // next enemy will spawn after at least this many seconds
    [SerializeField] float maxSpawnInterval = 4f;   // next enemy will spawn no longer than after this many seconds

    void Awake()
    {
        castleHealth = gameObject.GetComponent<Health>();
    }

    void Start()
    {
        // repeatedly spawn new enemy knights
        // NB! currently isn't working in set intervals
        InvokeRepeating("SpawnKnight", 2f, maxSpawnInterval);
    }

    void SpawnKnight()
    {
        GameObject newKnight = Instantiate(enemyKnight, spawnPosition.transform);

        // get new unit component
        WaypointFollower WaypointFollowerScript = newKnight.GetComponent<WaypointFollower>();

        // set new unit properties
        WaypointFollowerScript.SetTarget(targetPosition);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
       // check if the collider is a player unit
       if (other.gameObject.tag == "Player unit")
       {
            // take damage
            Debug.Log("enemy takes damage");
            castleHealth.Damage(castleDamage);



            // destroy enemy unit
            //castleHealth.Damage();
       }
    }

    // TO-DO: function for spawning enemy units
    // TO-DO: when health==0, call victory function

}
