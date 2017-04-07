using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationController : MonoBehaviour {

    private Animator animator;

    private EnemyStateControl enemyState;

    private enum AnimState { Idle, Attacking, Running }

    private AnimState currentAnim;

	// Use this for initialization
	void Awake () {
        enemyState = GetComponent<EnemyStateControl>();
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

    public void AnimateStun()
    {
        animator.SetBool("stunned", true);
        enemyState.navMeshAgent.Stop();
        enemyState.isStunned = true;
    }

    public void EndStunAnimation()
    {
        animator.SetBool("stunned", false);
        enemyState.navMeshAgent.Resume();
        enemyState.isStunned = false;
        animator.SetInteger("rand", Random.Range(0, 101));
        switch (currentAnim)
        {
            case AnimState.Running:
                animator.SetTrigger("running");
                break;
            case AnimState.Attacking:
                animator.SetTrigger("attacking");
                break;
            default:
                animator.SetTrigger("idle");
                break;
        }
    }
}
