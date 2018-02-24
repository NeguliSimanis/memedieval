using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
    public enum Type { Peasant, Knight, Archer, defaultType }

    #region assets
    public FMODUnity.StudioEventEmitter AttackSound;
    public Arrow ArrowPrefab; 
    #endregion

    [SerializeField] private Type UnitType;
    [SerializeField] private bool isArcher;
    [SerializeField] private bool isEnemy;

    #region damage variables
    [SerializeField] private int Damage;
    private float damageModifier = 1f;
    private bool hasDamageModifier = false;
    #endregion

    private List<Health> targets;
    private float cooldown;
    private GameObject playerObject;
    private PlayerProfile playerProfile;
    private string playerProfileTag = "Player profile"; // the same profile must be set in the scene

    void Start()
    {
        targets = new List<Health>();
		if (gameObject.GetComponent<FMODUnity.StudioEventEmitter> ()) 
		{
			AttackSound = GetComponent<FMODUnity.StudioEventEmitter> ();
		}

        #region check for active attack modifiers
        if (!isEnemy)
        {
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
        #endregion
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
        if (cooldown > 0) cooldown -= Time.deltaTime;
        else if (targets.Count > 0) Strike();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) return;
        if ((isEnemy && (collision.gameObject.tag == "Player unit" || collision.gameObject.tag == "Player castle")) ||
            (!isEnemy && (collision.gameObject.tag == "Enemy unit" || collision.gameObject.tag == "EnemyCastle")))
        {
            Health target = collision.gameObject.GetComponent<Health>();
            if (target) targets.Add(target);
        }
    }


    void OnTriggerExit2D(Collider2D collision)
    {
        Health target = collision.gameObject.GetComponent<Health>();
        if (target) targets.Remove(target);
    }


    public int TargetAmount()
    {
        return targets.Count;
    }


    // Deals damage to target
    private void Strike()
    {
        if (hasDamageModifier && !isEnemy)
        {
            if (isArcher)
            {
                Arrow arrow = Instantiate(ArrowPrefab, transform.position, Quaternion.identity);
                arrow.Damage = (int)(Damage * damageModifier);
                arrow.Target = StrikeUnitsFirst().gameObject.transform;
            }
            else
                StrikeUnitsFirst().Damage(UnitType, (int)(Damage * damageModifier));
        }
        else // damage is dealt by an enemy unit
        {
            if (isArcher)
            {
                Arrow arrow = Instantiate(ArrowPrefab, transform.position, Quaternion.identity);
                arrow.Damage = Damage;
                arrow.Target = StrikeUnitsFirst().gameObject.transform;
            }
            else
                StrikeUnitsFirst().Damage(UnitType, Damage);  
        }
        cooldown = 0.5f;    
    }


    private Health StrikeUnitsFirst()
    {
        
		if (AttackSound != null) {
			AttackSound.Play ();
		}

		if (( (isEnemy && (targets[0].gameObject.tag.Equals("Player castle"))) ||
              (!isEnemy && (targets[0].gameObject.tag.Equals("EnemyCastle"))) ) && targets.Count > 1)
        {
            //Debug.Log("Castle was not attacked.");
            return targets[1];
        }
        //Debug.Log(!isEnemy + ", "  + (targets[0].gameObject.tag.Equals("EnemyCastle")) + ", " + targets.Count + ", " + targets[0].gameObject.tag);
        return targets[0];
    }
}

