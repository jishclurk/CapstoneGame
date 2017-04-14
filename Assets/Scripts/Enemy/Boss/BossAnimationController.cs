using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationController : MonoBehaviour {

    private Animator animator;

    private enum AnimState { Idle, Sweep, Explode, Laser, Spawn, TurnLeft, TurnRight }

    private AnimState currentAnim;

    void Awake () {
        animator = GetComponent<Animator>();
        currentAnim = AnimState.Idle;
    }

    public void AnimateDeath()
    {
        animator.SetBool("dead", true);
    }

    public void AnimateIdle()
    {
        if (currentAnim != AnimState.Idle)
        {
            animator.SetTrigger("idle");
            currentAnim = AnimState.Idle;
        }
    }

    public void AnimateSweepAttack()
    {
        if (currentAnim != AnimState.Sweep)
        {
            animator.SetTrigger("sweep");
            currentAnim = AnimState.Sweep;
        }
    }

    public void AnimateExplodeAttack()
    {
        if (currentAnim != AnimState.Explode)
        {
            animator.SetTrigger("explosion");
            currentAnim = AnimState.Explode;
        }
    }

    public void AnimateLaserAttack()
    {
        if (currentAnim != AnimState.Laser)
        {
            animator.SetTrigger("laser");
            currentAnim = AnimState.Laser;
        }
    }

    public void AnimateSpawnAttack()
    {
        if (currentAnim != AnimState.Spawn)
        {
            animator.SetTrigger("spawn");
            currentAnim = AnimState.Spawn;
        }
    }

    public void AnimateLeftTurn()
    {
        if (currentAnim != AnimState.TurnLeft)
        {
            animator.SetTrigger("left");
            currentAnim = AnimState.TurnLeft;
        }
    }

    public void AnimateRightTurn()
    {
        if (currentAnim != AnimState.TurnRight)
        {
            animator.SetTrigger("right");
            currentAnim = AnimState.TurnRight;
        }
    }
}
