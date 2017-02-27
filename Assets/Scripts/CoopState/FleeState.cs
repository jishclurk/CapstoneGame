using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : ICoopState
{

    private readonly CoopAiController aiPlayer;
    private float animSpeed;

    public FleeState(CoopAiController coopAi)
    {
        aiPlayer = coopAi;
    }

    public void UpdateState()
    {
        FollowActivePlayer();
        //aiPlayer.CheckForCombat();
        aiPlayer.animController.AnimateMovement(animSpeed);
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
            animSpeed = aiPlayer.walkSpeed;
        }
        else
        {
            aiPlayer.navMeshAgent.Stop();
            animSpeed = 0.0f;
            if(aiPlayer.tm.activePlayer.visibleEnemies.Count > 0)
            {
                ToAttackState();
            }
            if (aiPlayer.visibleEnemies.Count == 0) { //jitter fix
                ToIdleState();
            }            

           
        }
    }

}
