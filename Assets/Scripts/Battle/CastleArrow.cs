using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleArrow : MonoBehaviour {
    #region targetting
    [SerializeField]
    private string arrowTargetLocationTag = "Castle arrow target";
    [SerializeField]
    private string playerUnitTag = "Player unit";
    [SerializeField]
    Transform[] targetArray;
    public Transform Target;
    private bool isActive = true;

    #endregion

    #region properties
    [SerializeField]
    private float Speed;
    public int damage;
    [SerializeField]
    float shootingAngle = 20f;
    Rigidbody2D rigidbody2d;
    #endregion

    #region selfdestruction
    private float arrowBirthTime;
    private float disablingCooldown = 0.1f;
    private float destructionDelay = 3f;
    #endregion

    void Start()
    {
        SetArrowDamage();
        arrowBirthTime = Time.time;
        SelectTarget();
        rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
        rigidbody2d.velocity = CalculateArrowVelocity(shootingAngle);
    }

    void SetArrowDamage()
    {
        Debug.Log("Arrow damage before modifiers: " + damage);
        ChampionEffect championEffect = PlayerProfile.Singleton.gameObject.GetComponent<ChampionEffect>();

        damage = Mathf.RoundToInt(championEffect.castleArrowDamageCoefficient * damage);
        if (damage < championEffect.minCastleArrowDamage)
            damage = championEffect.minCastleArrowDamage;
        Debug.Log("Arrow damage after modifiers: " + damage);
    }

    void SelectTarget()
    {
        // fill target array if empty
        if (targetArray[0] == null)
        {
            GameObject[] tempArray = GameObject.FindGameObjectsWithTag(arrowTargetLocationTag);
            int counter = 0;
            foreach (GameObject obj in tempArray)
            {
                targetArray[counter] = obj.GetComponent<Transform>();
                counter++;
            }
        }

        // select a random target
        int targetID = Random.Range(0, targetArray.Length);
        Target = targetArray[targetID];
    }

    Vector2 CalculateArrowVelocity(float angle)
    {

        Vector2 targetDirection = new Vector2(Target.position.x - transform.position.x, Target.position.y - transform.position.y);

        float heightDifference = targetDirection.y;

        // retain only the horizontal direction
        targetDirection.y = 0;

        // get horizontal distance
        float horizontalDistance = targetDirection.magnitude;

        // convert angle to radians
        float angleInRadians = angle * Mathf.Deg2Rad;

        // set targetdirection to the elevation angle
        targetDirection.y = horizontalDistance * Mathf.Tan(angleInRadians);

        // correct for small height differences
        if (Mathf.Tan(angleInRadians) == 0) Debug.Log("ERROR");
        horizontalDistance += heightDifference / Mathf.Tan(angleInRadians);

        // calculate the velocity magnitude
        if (horizontalDistance * Physics.gravity.magnitude == 0) Debug.Log("ERROR");
        if (Mathf.Sin(2 * angleInRadians) == 0) Debug.Log("ERROR");
        var vel = Mathf.Sqrt(horizontalDistance * Physics.gravity.magnitude / Mathf.Sin(2 * angleInRadians));

        if (vel * targetDirection.normalized == null) Debug.Log("error");

        Vector2 result = vel * targetDirection.normalized;

        //check for errors in result
        if (float.IsNaN(result.x) || float.IsNaN(result.y))
        {
            Debug.Log("physics error");
            return new Vector2(-5.268912f, 6.279245f); // return default value
        }
        else
        {
            //Debug.Log(result.x);
            //Debug.Log(result.y);
            return result;
        }
    }

    void Update()
    {
        if (!isActive)
        {
            if (Time.time >= arrowBirthTime + destructionDelay)
                Destroy(gameObject);
            return;
        }
        // rotate the arrow according to its current velocit
        Vector3 dir = gameObject.GetComponent<Rigidbody2D>().velocity;
        float angle = Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.tag == playerUnitTag && isActive)
        {
            other.gameObject.GetComponent<Health>().Damage(damage, Attack.Type.Archer);
            Destroy(gameObject);
        }
    }   

    void OnCollisionEnter2D()
    {
        if (Time.time >= arrowBirthTime + disablingCooldown)
        {
            Destroy(rigidbody2d);
            isActive = false;
        }
    }
}
