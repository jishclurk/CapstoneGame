using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IPlayerState {

    private readonly StatePatternPlayer player;

    public AttackState(StatePatternPlayer statePatternPlayer)
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
        player.currentState = player.idleState;
    }

    public void ToCastState()
    {
        player.currentState = player.castState;
    }

    private void MoveWithinRange()
    {
        //Movement can be greatly improved, just a proof of concept at the moment
        if (Vector3.Distance(player.transform.position, player.attackTargetPosition) > player.selectedAbility.range)
        {
            Vector3 forward = player.moveDestination - player.transform.position;
            player.transform.Translate(forward * 0.01f);
        }
        else
        {
            Debug.Log("Withing range, shots fired. Damage done: " + player.selectedAbility.baseDamage);
            ToIdleState(); //Perhaps repeating attack here?
        }
    }
}
