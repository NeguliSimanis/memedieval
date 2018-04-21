using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatorSpeed : MonoBehaviour {
    Animator animator;
    private float currentAnimSpeed;

	void Start ()
    {
        animator = gameObject.GetComponent<Animator>();
	}
	
	public void ChangeAnimSpeed(float amount)
    {
        animator.speed = animator.speed + amount;
    }
}
