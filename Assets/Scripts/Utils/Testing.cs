using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

    [SerializeField]
    bool allowGamePause = true;
    [SerializeField]
    bool spawnEnemyUnits = true;
    [SerializeField]
    bool shootCastleArrows = true;

    [SerializeField]
    EnemyCastleController enemySpawner;
    [SerializeField]
    EnemyController castleArrowController;

    bool isPaused = false;

    void Start()
    {
        /*GameObject[] gameControllers = GameObject.FindGameObjectsWithTag("GameController");
        foreach (GameObject gameController in gameControllers)
        {
            Debug.Log(gameController.name);
        }*/

        if (!spawnEnemyUnits)
        {
            enemySpawner.enabled = false;
        }
        else
            enemySpawner.enabled = true;

        if (shootCastleArrows)
            castleArrowController.enabled = true;
        else
            castleArrowController.enabled = false;
    }

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
    }
}
