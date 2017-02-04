using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : ICoopState
{

    private readonly CoopAiController player;

    public AttackState(CoopAiController statePatternPlayer)
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
