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
        AnimationEvent evt = new AnimationEvent();
        evt.functionName = "FootStep";
        AnimationClip clip = animator.runtimeAnimatorController.animationClips[0];
        clip.AddEvent(evt);
    }

    public void AnimateDeath()
    {
        if (!deathAnimated)
        {
            animator.SetFloat("Y", 0.0f);
            animator.SetBool("Dead", true);
            deathAnimated = true;
        }
    }

    public void AnimateMovement(float speed)
    {
        if (!deathAnimated)
        {
            animator.SetFloat("Y", 0.0f);
            animator.SetBool("OnGround", true);
            animator.SetFloat("Speed", speed);
            animator.SetBool("Aiming", false);
        }
    }

    public void AnimateIdle()
    {
        if (!deathAnimated)
        {
            animator.SetFloat("Y", 0.0f);
            animator.SetBool("OnGround", true);
            animator.SetFloat("Speed", 0.0f);
        }
    }

    public void AnimateAimChasing()
    {
        if (!deathAnimated)
        {
            animator.SetFloat("Y", 1.0f);
            animator.SetBool("OnGround", true);
            animator.SetFloat("Speed", 0.0f);
            animator.SetBool("Aiming", true);
        }
    }

    public void AnimateAimStanding()
    {
        if (!deathAnimated)
        {
            animator.SetFloat("Y", 0.0f);
            animator.SetBool("OnGround", true);
            animator.SetFloat("Speed", 0.0f);
            animator.SetBool("Aiming", true);
        }
    }

    public void AnimateShoot()
    {
        if (!deathAnimated)
        {
            animator.SetBool("OnGround", true);
            animator.SetFloat("Speed", 0.0f);
            animator.SetBool("Aiming", true);
            animator.SetTrigger("Shoot");
        }
    }

    public void AnimateUse(float duration)
    {
        if (!deathAnimated)
        {
            animator.SetBool("OnGround", true);
            animator.SetFloat("Speed", 0.0f);
            animator.SetBool("Aiming", true);
            animator.SetTrigger("Use");
            StartCoroutine(StopUse(duration));
        }
    }

    private IEnumerator StopUse(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (!deathAnimated)
        {
            animator.SetBool("OnGround", true);
            animator.SetFloat("Speed", 0.0f);
            animator.SetBool("Aiming", true);
            animator.SetTrigger("StopUse");
        }

    }

    private void FootStep()
    {
        //here is where we can put footstep related things if we want
    }

}
