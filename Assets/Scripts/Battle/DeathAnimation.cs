using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Play animation attached to object and destroy it alongside parent object
/// </summary>
public class DeathAnimation : MonoBehaviour
{
    bool isWaitingDeath = false;
    float deathTime;
    [SerializeField]
    AnimationClip deathAnim;

    public void PlayDeath()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Die");
        deathTime = Time.time + deathAnim.length;
        isWaitingDeath = true;
    }

    private void Update()
    {
        if (!isWaitingDeath)
            return;
        if (Time.time > deathTime)
            Destroy(gameObject.transform.parent.gameObject);
    }
}
