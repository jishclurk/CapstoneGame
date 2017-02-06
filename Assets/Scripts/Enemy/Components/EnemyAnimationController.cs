using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour {

    private Animator animator;
    private bool deathAnimated;

	// Use this for initialization
	void Awake () {
        animator = GetComponent<Animator>();
	}

    public void AnimateDeath()
    {
        if (!deathAnimated)
        {
            animator.SetBool("Moving", false);
            animator.SetBool("Sprinting", false);
            animator.SetInteger("Action", 0);
            animator.SetInteger("Death", 2);
            deathAnimated = true;
        }
    }

    public void AnimateMovement()
    {
        if (!deathAnimated)
        {
            animator.SetBool("Moving", true);
            animator.SetBool("Sprinting", true);
            animator.SetInteger("Action", 0);
            animator.SetInteger("Death", 0);
        }
    }

    public void AnimateIdle()
    {
        if (!deathAnimated)
        {
            animator.SetBool("Moving", false);
            animator.SetBool("Sprinting", false);
            animator.SetInteger("Action", 0);
            animator.SetInteger("Death", 0);
        }
    }

    public void AnimateAttack()
    {
        if (!deathAnimated)
        {
            animator.SetBool("Moving", false);
            animator.SetBool("Sprinting", false);
            animator.SetInteger("Action", 1);
            animator.SetInteger("Death", 0);
        }
    }
}
