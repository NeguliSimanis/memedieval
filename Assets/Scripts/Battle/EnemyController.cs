using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    GameObject enemyObject;
    [SerializeField]
    Transform enemyTransform;

    #region castle arrows
    [Header("Castle arrows")]
    [SerializeField]
    CastleArrow castleArrowPrefab;
    [SerializeField]
    int arrowDamage = 10;
    [SerializeField]
    float minArrowSpawnDelay = 3f;
    [SerializeField]
    float maxArrowSpawnDelay = 6f;
    float arrowSpawnDelay;
    [SerializeField]
    Transform arrowTarget;

    float arrrowShootTime;
    #endregion

    #region tutorial
    [Header("Tutorial")]
    [SerializeField]
    TutorialArrow tutorialArrowPrefab;
    [SerializeField]
    bool isTutorial;
    #endregion

    void Start ()
    {
        arrowSpawnDelay = maxArrowSpawnDelay;
        arrrowShootTime = Time.time + arrowSpawnDelay;
    }
	
    void ShootArrow()
    {
        CastleArrow arrow = Instantiate(castleArrowPrefab, transform.position, Quaternion.identity);
        //arrow.Damage = arrowDamage;
        arrow.damage = arrowDamage;
        arrow.Target = arrowTarget;
    }

    void Update ()
    {
		if (Time.time >= arrrowShootTime)
        {
            ShootArrow();
            arrowSpawnDelay = Random.Range(minArrowSpawnDelay, maxArrowSpawnDelay);
            arrrowShootTime = Time.time + arrowSpawnDelay;
        }
	}
}
