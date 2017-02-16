using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : ICoopState
{

    private readonly CoopAiController aiPlayer;

    public FleeState(CoopAiController coopAi)
    {
        aiPlayer = coopAi;
    }

    public void UpdateState()
    {
        FollowActivePlayer();
        //aiPlayer.CheckForCombat();
        aiPlayer.anim.SetBool("Idling", false);
        aiPlayer.anim.SetBool("NonCombat", false);
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
        Debug.Log("Cannot go to Flee state from Flee State");
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
            aiPlayer.navMeshAgent.Stop();
            ToIdleState();

           
        }
    }

}
