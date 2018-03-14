using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float Speed;
    private float SelfDestruct;
    public Transform Target;
    private int damage;
    public bool isEnemy = false;
    private string enemyCastleTag = "EnemyCastle";

    void Start()
    {
        SelfDestruct = 0;
    }


    void Update()
    {
        if (Target == null)
        {
            SelfDestruct += Time.deltaTime;
            if (SelfDestruct >= 0.3f) GameObject.Destroy(gameObject);
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, Target.transform.position) <= float.Epsilon)
        {
            if (isEnemy || Target.gameObject.tag == enemyCastleTag)
                Target.GetComponent<Health>().Damage(Damage, Attack.Type.Archer);

            else if (!isEnemy)
                Target.GetComponent<EnemyUnit>().Damage(PlayerUnit.Type.Archer, Damage);
            GameObject.Destroy(gameObject);
        }
    }

  /*  void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject == Target.gameObject)
        { 
            Destroy(gameObject);
            Target.GetComponent<Health>().Damage(damage, Attack.Type.Archer);
        }
    }*/

    public int Damage
    {
        get { return damage; }
        set
        {
            if (damage <= 0)
            {
                damage = value;
            }
        }
    }
}

