using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

    [SerializeField]
    bool allowGamePause = true;
    [SerializeField]
    bool spawnEnemyUnits = true;
    [SerializeField]
    EnemyCastleController enemySpawner;
    bool isPaused = false;

    void Start()
    {
        if (!spawnEnemyUnits)
        {
            enemySpawner.enabled = false;
        }
        else
            enemySpawner.enabled = true;
    }

	void Update ()
    {
        if (Input.GetKey(KeyCode.P))
        {
            if (isPaused == false)
            {
                isPaused = true;
                Time.timeScale = 0;
            }
            else
            {
                isPaused = false;
                Time.timeScale = 1;
            }
        }
    }
}
