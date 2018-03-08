using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBalancer : MonoBehaviour
{
    /* not all variables are implemented yet
     */

    #region variable declarations

    public int currentCastleID; // set by Map

    #region unitProperties
    [Header("Enemy units")]
    [SerializeField]
    int enemyArcherHealth; //TO-DO
    [SerializeField]
    int enemyPeasantHealth; //TO-DO
    [SerializeField]
    int enemyKnightHealth; //TO-DO

    int enemyArcherDamage; //TO-DO
    int enemyPeasantDamage; //TO-DO
    int enemyKnightDamage; //TO-DO

    float enemyArcherMoveSpeed; //TO-DO
    float enemyPeasantMoveSpeed; //TO-DO
    float enemyKnightMoveSpeed; //TO-DO
    #endregion

    #region unit spawning
    [Header("Enemy unit spawning")]
    [SerializeField]
    bool spawnEnemies; //TO-DO

    [SerializeField][Range(1,2000)]
    float minArcherSpawnDelay; //TO-DO
    [SerializeField][Range(1, 2000)]
    float maxArcherSpawnDelay; //TO-DO

    [SerializeField][Range(1, 2000)]
    float minPeasantSpawnDelay; //TO-DO
    [SerializeField][Range(1, 2000)]
    float maxPeasantSpawnDelay; //TO-DO

    [SerializeField][Range(1, 2000)]
    float minKnightSpawnDelay; //TO-DO
    [SerializeField][Range(1, 2000)]
    float maxKnightSpawnDelay; //TO-DO
    #endregion

    #region enemy castle
    [Header("Enemy castle")]
    [SerializeField][Range(1,2000)]
    int castleMaxHealth; //TO-DO
   // int castleHealth; //TO-DO
    #endregion

    #region castle arrows
    [Header("Castle arrows")]
    [SerializeField]
    bool shootArrows; //TO-DO

    [SerializeField][Range(0, 200)]
    int arrowDamage; //TO-DO

    [SerializeField]
    [Range(-90, 90)]
    float arrowAngle; //TO-DO

    [SerializeField][Range(0, 200)]
    float minShootDelay; //TO-DO

    [SerializeField][Range(1, 200)]
    float maxShootDelay; //TO-DO
    #endregion
    #endregion


    void OnEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
