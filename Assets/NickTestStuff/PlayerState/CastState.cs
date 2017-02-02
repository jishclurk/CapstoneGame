using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastState : IPlayerState
{

    private readonly StatePatternPlayer player;

    public CastState(StatePatternPlayer statePatternPlayer)
    {
        player = statePatternPlayer;
    }

    public void UpdateState()
    {

    }

    public void ToMoveState()
    {

    }

    public void ToAttackState()
    {

    }

    public void ToIdleState()
    {

    }

    public void ToCastState()
    {

    }
}
