using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float Speed;
    private float SelfDestruct;
    public Transform Target;
    private int damage;


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
            Target.GetComponent<Health>().Damage(Damage, Attack.Type.Archer);
            Target.GetComponent<EnemyUnit>().Damage(PlayerUnit.Type.Archer, Damage);
            GameObject.Destroy(gameObject);
        }
    }


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

