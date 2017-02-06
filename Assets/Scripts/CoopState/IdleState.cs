using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : ICoopState
{
    private readonly CoopAiController aiPlayer;

    public IdleState (CoopAiController statePatternPlayer)
    {
        aiPlayer = statePatternPlayer;
    }


    public void UpdateState()
    {
        Debug.Log("IDLE");
        WatchActivePlayer();
        aiPlayer.CheckForCombat();
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
        aiPlayer.navMeshAgent.Stop();
        GameObject userPlayer = aiPlayer.tm.activePlayer;
        if (userPlayer == null)
        {
            //this return happens if player dies
            return; //avoid running code we don't need to.
        }
       
        float remainingDistance = Vector3.Distance(userPlayer.transform.position, aiPlayer.transform.position);
        if (remainingDistance >= aiPlayer.followDist + aiPlayer.followEpsilon)
        {
            aiPlayer.transform.LookAt(userPlayer.transform);
            ToMoveState();
        }
        if (aiPlayer.tm.isTeamInCombat)
        {
            ToAttackState();
        }
    }

}
