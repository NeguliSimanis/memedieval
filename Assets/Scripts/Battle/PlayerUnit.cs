using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour {
    /*
     * handles attacking
     */

    #region variables
    #region unit identity
    public enum Type { Peasant, Knight, Archer, defaultType }
    [SerializeField]
    private Type UnitType;
    [SerializeField]
    private bool isArcher;
    #endregion

    #region assets
    public FMODUnity.StudioEventEmitter AttackSound;
    public Arrow ArrowPrefab;
    #endregion

    #region damage variables
    [SerializeField]
    private int Damage;
    private float damageModifier = 1f;
    private bool hasDamageModifier = false;
    #endregion

    #region targetting
    [SerializeField]
    public float defaultCooldown;
    private float cooldown;
    private List<EnemyUnit> targets;

    private string enemyCastleTag = "EnemyCastle";
    private Health enemyCastle;
    private bool isNearCastle = false;

    #endregion

    private GameObject playerObject;
    private PlayerProfile playerProfile;
    private string playerProfileTag = "Player profile"; // the same profile must be set in the scene
    #endregion

    void Start()
    {
        targets = new List<EnemyUnit>();
        if (gameObject.GetComponent<FMODUnity.StudioEventEmitter>())
        {
            AttackSound = GetComponent<FMODUnity.StudioEventEmitter>();
        }


        if (GameObject.FindGameObjectWithTag(playerProfileTag))
        {
            playerObject = GameObject.FindGameObjectWithTag(playerProfileTag);
            playerProfile = playerObject.GetComponent<PlayerProfile>();
            if (playerProfile.isDrunk)
            {
                SetModifiers();
            }
        }
        else
        {
            Debug.Log("No player profile set in the scene!");
        }
    }

    private void SetModifiers()
    {
        // set hangover modifier
        hasDamageModifier = true;
        Hangover hangover = playerObject.GetComponent<Hangover>();
        damageModifier = 1f + hangover.hangoverBoost;
        Debug.Log("Hangover damage boost: " + damageModifier);

    }

    void Update()
    {
        if (Health.Victory || Health.GameOver) return;
        if (cooldown > 0)
        {
            Debug.Log("cooldown");
            cooldown -= Time.deltaTime;
        }
        else if (targets.Count > 0 || isNearCastle)
        {
            Strike();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
       // if (collision.isTrigger) return;

        if (collision.gameObject.tag == "Enemy unit")
        {
            EnemyUnit target = collision.gameObject.GetComponent<EnemyUnit>();
            if (target) targets.Add(target);
        }

        if (collision.gameObject.tag == enemyCastleTag)
        {
            Debug.Log("near castle");
            enemyCastle = collision.gameObject.GetComponent<Health>();
            isNearCastle = true;
        }
    }


    void OnTriggerExit2D(Collider2D collision)
    {
        EnemyUnit target = collision.gameObject.GetComponent<EnemyUnit>();
        if (target) targets.Remove(target);
    }

    // if returns more than 0, unit will stop moving to attack
    public int TargetAmount()
    {
        if (targets.Count == 0 && isNearCastle)
            return 1;
        return targets.Count;
    }


    // Deals damage to target
    private void Strike()
    {
        if (isArcher)
        {
            Arrow arrow = Instantiate(ArrowPrefab, transform.position, Quaternion.identity);
            arrow.Damage = (int)(Damage * damageModifier);

            if (targets.Count > 0)
                arrow.Target = targets[0].gameObject.transform;
            else arrow.Target = enemyCastle.gameObject.transform;
        }
        else
        {
            if (targets.Count > 0)
            {
                Debug.Log("Damaging units");
                targets[0].Damage(UnitType, Damage);
            }
            else
            {
                Debug.Log("Damaging castle");
                enemyCastle.Damage(Damage);
            }
        }
 
        cooldown = defaultCooldown;
    }



}
