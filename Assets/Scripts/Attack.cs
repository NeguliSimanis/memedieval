using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
    public enum Type { Peasant, Knight, Archer, defaultType }

    [SerializeField] private Type UnitType;
    [SerializeField] private bool isArcher;
    [SerializeField] private bool isEnemy;
    [SerializeField] private int Damage;

    private List<Health> targets;
    private float cooldown;
	public FMODUnity.StudioEventEmitter AttackSound;
    public Arrow ArrowPrefab;


    void Start()
    {
        targets = new List<Health>();
		if (gameObject.GetComponent<FMODUnity.StudioEventEmitter> ()) 
		{
			AttackSound = GetComponent<FMODUnity.StudioEventEmitter> ();
		}

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


    private void Strike()
    {
		
		
		if (isArcher)
        {
            Arrow arrow = Instantiate(ArrowPrefab, transform.position, Quaternion.identity);
            arrow.Damage = Damage;
            arrow.Target = StrikeUnitsFirst().gameObject.transform;
        }
        else StrikeUnitsFirst().Damage(UnitType, Damage);
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

