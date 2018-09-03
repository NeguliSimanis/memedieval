using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Forces death animation to play and destroys the death animation object after a cooldown
/// Script created in attempt to fix bug where the death animation sometimes doesn't play.
/// 
/// 04.09.2018
/// </summary>
public class DeathAnimationCleanup : MonoBehaviour
{

    [SerializeField] AnimationClip deathAnimation;

	void Start ()
    {
        Animator animator = gameObject.GetComponent<Animator>();
        animator.Play(deathAnimation.name);
        Destroy(gameObject, deathAnimation.length);	
	}

}
