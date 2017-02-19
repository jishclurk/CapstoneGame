using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour {

    private Animator animator;

    private enum AnimState { Idle, Attacking, Running}

    private AnimState currentAnim;

	// Use this for initialization
	void Awake () {
        animator = GetComponent<Animator>();
        currentAnim = AnimState.Idle;
    }

    public void AnimateDeath()
    {
        animator.SetBool("dead", true);
    }

    public void AnimateMovement()
    {
        if (currentAnim != AnimState.Running)
        {
            animator.SetTrigger("running");
            currentAnim = AnimState.Running;
        }
    }

    public void AnimateIdle()
    {
        if (currentAnim != AnimState.Idle)
        {
            animator.SetTrigger("idle");
            currentAnim = AnimState.Idle;
        }
    }

    public void AnimateAttack()
    {
        if (currentAnim != AnimState.Attacking)
        {
            animator.SetTrigger("attacking");
            currentAnim = AnimState.Attacking;
        }
    }
}
