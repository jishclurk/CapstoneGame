﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : ICoopState
{

    private readonly CoopAiController aiPlayer;

    public MoveState(CoopAiController coopAi)
    {
        aiPlayer = coopAi;
    }

    public void UpdateState()
    {
        FollowActivePlayer();
        aiPlayer.CheckForCombat();
        aiPlayer.animController.AnimateMovement(aiPlayer.walkSpeed);
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

    public void ToFleeState()
    {
        aiPlayer.currentState = aiPlayer.fleeState;
    }

    private void FollowActivePlayer()
    {
        GameObject userPlayer = aiPlayer.tm.activePlayer.gameObject;
        if (userPlayer == null)
        {
            //this return happens if enemy dies
            return; //avoid running code we don't need to.
        }
        aiPlayer.navMeshAgent.destination = userPlayer.transform.position;
        float remainingDistance = Vector3.Distance(userPlayer.transform.position, aiPlayer.transform.position);
        if (remainingDistance >= aiPlayer.followDist)
        {
            aiPlayer.navMeshAgent.Resume();
        }
        else
        {
            ToIdleState();
        }
    }

}