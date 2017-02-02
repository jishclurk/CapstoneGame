using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : IPlayerState
{

    private readonly StatePatternPlayer player;

    public MoveState(StatePatternPlayer statePatternPlayer)
    {
        player = statePatternPlayer;
    }

    public void UpdateState()
    {
        player.HandleInput();
        Move();

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

    private void Move()
    {
        //Movement can be greatly improved, just a proof of concept at the moment
        if (Vector3.Distance(player.transform.position, player.moveDestination) > 1.0f)
        {
            Vector3 forward = player.moveDestination - player.transform.position;
            player.transform.Translate(forward * 0.01f);
        } else
        {
            Debug.Log("Destination reached, IDLING!");
            ToIdleState();
        }

    }

}
