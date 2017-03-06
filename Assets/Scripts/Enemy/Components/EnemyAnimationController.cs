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
        animator.SetInteger("rand", Random.Range(0, 101));
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
            animator.SetInteger("rand", Random.Range(0, 101));
            currentAnim = AnimState.Idle;
        }
    }

    public void AnimateAttack()
    {
        if (currentAnim != AnimState.Attacking)
        {
            animator.SetInteger("rand", Random.Range(0, 101));
            animator.SetTrigger("attacking");
            currentAnim = AnimState.Attacking;
        }
    }
}
