using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleArrow : MonoBehaviour {
    [SerializeField]
    private string playerUnitTag = "Player unit";
    [SerializeField]
    private float Speed;
    private float SelfDestruct;
    public Transform Target;
    [SerializeField]
    private int damage;


    void Start()
    {
        SelfDestruct = 0;
        //gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(-1f,1f);
    }


    void Update()
    {
        /*Vector3 dir = gameObject.GetComponent<Rigidbody2D>().velocity;
        float angle = Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);*/

        if (Target == null)
        {
            SelfDestruct += Time.deltaTime;
            if (SelfDestruct >= 0.3f) GameObject.Destroy(gameObject);
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, Target.transform.position) <= float.Epsilon)
        {
            GameObject.Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("collision0");
        if (other.gameObject.tag == playerUnitTag)
        {
            Debug.Log("collision1");
            Destroy(other.gameObject);
            Target.GetComponent<Health>().Damage(Attack.Type.Archer, damage);
        }
    }   
}
