using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : ICoopState
{

    private readonly PlayerControllerMachine aiPlayer;

    public PlayerMoveState(PlayerControllerMachine coopAi)
    {
        aiPlayer = coopAi;
    }

    public void UpdateState()
    {
        FollowActivePlayer();
        aiPlayer.anim.SetBool("Idling", false);
        aiPlayer.anim.SetBool("NonCombat", true);
    }


    public void ToMoveState()
    {
        aiPlayer.currentState = aiPlayer.moveState;
    }

    public void ToAttackState()
    {
        aiPlayer.currentState = aiPlayer.attackState;
    }

    public void ToIdleState()
    {
        aiPlayer.currentState = aiPlayer.idleState;
    }

    public void ToCastState()
    {
        aiPlayer.currentState = aiPlayer.castState;

    }

    private void FollowActivePlayer()
    {

    }

}
