using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : ICoopState
{

    private readonly PlayerControllerMachine player;

    public PlayerAttackState(PlayerControllerMachine statePatternPlayer)
    {
        player = statePatternPlayer;
    }

    public void UpdateState()
    {
        
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
        player.currentState = player.idleState;
    }

    public void ToCastState()
    {
        player.currentState = player.castState;
    }

}
