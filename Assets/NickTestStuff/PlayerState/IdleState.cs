using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IPlayerState
{
    private readonly StatePatternPlayer player;

    public IdleState (StatePatternPlayer statePatternPlayer)
    {
        player = statePatternPlayer;
    }


    public void UpdateState()
    {
        player.HandleInput();
    }

    public void ToMoveState()
    {
        player.currentState = player.moveState;
    }

    public void ToAttackState()
    {
        player.currentState = player.attackState;
    }

    public void ToIdleState()
    {
        Debug.Log("Can't transition to same state Idle.");
    }

    public void ToCastState()
    {
        player.currentState = player.castState;
    }


}
