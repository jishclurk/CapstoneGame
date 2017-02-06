using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCastState : ICoopState
{

    private readonly PlayerControllerMachine player;

    public PlayerCastState(PlayerControllerMachine statePatternPlayer)
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
