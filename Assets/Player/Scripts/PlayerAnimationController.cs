using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour {

    private Animator animator;
    private bool deathAnimated;
    private bool idleAnimated;

    // Use this for initialization
    void Awake()
    {
        animator = GetComponent<Animator>();
        AnimationEvent evt = new AnimationEvent();
        evt.functionName = "FootStep";
        AnimationClip clip = animator.runtimeAnimatorController.animationClips[0];
        clip.AddEvent(evt);

        AnimationEvent evt2 = new AnimationEvent();
        evt2.functionName = "EndPickup";
        AnimationClip clip2 = animator.runtimeAnimatorController.animationClips[1];
        clip2.AddEvent(evt2);


    }

    public void AnimateRevive()
    {
        deathAnimated = false;
        animator.Rebind();
        idleAnimated = false;
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
            idleAnimated = false;
            animator.SetFloat("Y", 0.0f);
            animator.SetBool("OnGround", true);
            animator.SetFloat("Speed", speed);
            animator.SetBool("Aiming", false);
        }
    }

    public void AnimateIdle()
    {
        if (!deathAnimated && !idleAnimated)
        {
            animator.SetFloat("Y", 0.00f);
            animator.SetFloat("Speed", 0.0f);
            animator.SetBool("Aiming", false);
            animator.SetBool("OnGround", true);
        }
    }

    /*public void AnimateIdle()
    {
        if (!deathAnimated && !idleAnimated)
        {
            idleAnimated = true;
            StopAllCoroutines();
            StartCoroutine(IdleRoutine());
        }
    }

    IEnumerator IdleRoutine()
    {
        
        animator.SetFloat("Y", 0.01f);
        animator.SetBool("Aiming", true);
        animator.SetBool("OnGround", true);
        yield return new WaitForSeconds(0.10f);
        animator.SetFloat("Y", 0.00f);
        animator.SetFloat("Speed", 0.0f);
        yield return new WaitForSeconds(2.5f);
        animator.SetBool("Aiming", false);

    }*/

    public void AnimateAimChasing()
    {
        if (!deathAnimated)
        {
            idleAnimated = false;
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
            idleAnimated = false;
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
            idleAnimated = false;
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
            idleAnimated = false;
            animator.SetBool("OnGround", true);
            animator.SetFloat("Speed", 0.0f);
            animator.SetBool("Aiming", true);
            StopAllCoroutines();
            StartCoroutine(UseAnimation(duration));
        }
    }

    private IEnumerator UseAnimation(float duration)
    {
        yield return new WaitForFixedUpdate();
        animator.SetTrigger("Use");
        yield return new WaitForSeconds(duration);
        if (!deathAnimated)
        {
            animator.SetBool("OnGround", true);
            animator.SetFloat("Speed", 0.0f);
            animator.SetBool("Aiming", true);
            animator.SetTrigger("StopUse");
        }

    }

    public void AnimatePickup()
    {
        if (!deathAnimated)
        {
            idleAnimated = false;
            animator.SetBool("OnGround", true);
            animator.SetFloat("Speed", 0.0f);
            animator.SetBool("Aiming", false);
            animator.SetTrigger("Pickup");
        }
    }

    private void FootStep()
    {
        //here is where we can put footstep related things if we want
    }

    private void EndPickup()
    {
        //here is where we can put pickup
    }

}
