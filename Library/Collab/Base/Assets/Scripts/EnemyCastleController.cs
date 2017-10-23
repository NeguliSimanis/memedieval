using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCastleController : MonoBehaviour
{
    public int castleHealth = 100;

    // variables for spawning enemies
    [SerializeField] GameObject enemyKnight;
    [SerializeField] GameObject spawnPosition;      // where enemy units will spawn
    [SerializeField] Waypoint targetPosition;       // where enemy units will go (waypoint)
    [SerializeField] float minSpawnInterval = 2f;   // next enemy will spawn after at least this many seconds
    [SerializeField] float maxSpawnInterval = 4f;   // next enemy will spawn no longer than after this many seconds

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


            // destroy enemy unit

       }
    }

    // TO-DO: function for spawning enemy units
    // TO-DO: when health==0, call victory function

}
