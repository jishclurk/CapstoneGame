using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : ICoopState
{
    private readonly PlayerControllerMachine aiPlayer;

    public PlayerIdleState (PlayerControllerMachine statePatternPlayer)
    {
        aiPlayer = statePatternPlayer;
    }


    public void UpdateState()
    {
        WatchActivePlayer();
        aiPlayer.anim.SetBool("Idling", true);
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
        Debug.Log("Can't transition to same state Idle.");
    }

    public void ToCastState()
    {
        aiPlayer.currentState = aiPlayer.castState;
    }

    private void WatchActivePlayer()
    {
    }

}
