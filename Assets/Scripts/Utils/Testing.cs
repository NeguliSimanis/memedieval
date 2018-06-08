using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

    [SerializeField]
    bool allowGamePause = true; 
    public bool spawnEnemyUnits = true;
    public bool shootCastleArrows = true;

    [SerializeField]
    EnemyCastleController enemySpawner;
    [SerializeField]
    EnemyController castleArrowController;

    bool isPaused = false;

	void Update ()
    {
        if (Input.GetKey(KeyCode.P) && allowGamePause)
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

        if (shootCastleArrows)
            castleArrowController.enabled = true;
        else
            castleArrowController.enabled = false;

        if (!spawnEnemyUnits)
        {
            enemySpawner.enabled = false;
        }
        else
            enemySpawner.enabled = true;
    }
}
