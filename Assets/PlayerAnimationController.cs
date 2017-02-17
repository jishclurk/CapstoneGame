using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour {

    private Animator animator;
    private bool deathAnimated;

    // Use this for initialization
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void AnimateDeath()
    {
        if (!deathAnimated)
        {
            animator.SetBool("Dead", true);
            deathAnimated = true;
        }
    }

    public void AnimateMovement(float speed)
    {
        if (!deathAnimated)
        {
            animator.SetBool("OnGround", true);
            animator.SetFloat("Speed", speed);
            animator.SetBool("Aiming", false);
        }
    }

    public void AnimateIdle()
    {
        if (!deathAnimated)
        {
            animator.SetBool("OnGround", true);
            animator.SetFloat("Speed", 0.0f);
        }
    }

    public void AnimateAim()
    {
        if (!deathAnimated)
        {
            animator.SetBool("OnGround", true);
            animator.SetFloat("Speed", 0.0f);
            animator.SetBool("Aiming", true);
        }
    }
}
