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
            animator.SetTrigger("death");
            deathAnimated = true;
        }
    }

    public void AnimateMovement()
    {
        if (!deathAnimated)
        {
            animator.SetTrigger("running");
        }
    }

    public void AnimateIdle()
    {
        if (!deathAnimated)
        {
            animator.SetTrigger("idle");
        }
    }

    public void AnimateAttack()
    {
        if (!deathAnimated)
        {
            animator.SetTrigger("attacking");
        }
    }
}
