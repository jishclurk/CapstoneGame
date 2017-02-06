using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastState : ICoopState
{

    private readonly CoopAiController player;

    public CastState(CoopAiController statePatternPlayer)
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
