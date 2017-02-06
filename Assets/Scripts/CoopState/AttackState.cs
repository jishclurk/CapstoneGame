using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : ICoopState
{

    private readonly CoopAiController aiPlayer;
    private bool walking;

    public AttackState(CoopAiController statePatternPlayer)
    {
        aiPlayer = statePatternPlayer;
    }

    public void UpdateState()
    {
        Debug.Log("Attack");
        if (aiPlayer.targetedEnemy == null)
        {
            LookForEnemy();
        } else
        {
            MoveAndShoot();
        }
        aiPlayer.anim.SetBool("Idling", walking);
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

    private void LookForEnemy()
    {
        RaycastHit hit;
        foreach (GameObject gb in aiPlayer.visibleEnemies)
        {
            Vector3 playerToEnemy = gb.transform.position - aiPlayer.transform.position;
            if (Physics.Raycast(aiPlayer.transform.position, playerToEnemy, out hit, aiPlayer.sightDist) && hit.collider.CompareTag("Enemy"))
            {
                aiPlayer.targetedEnemy = hit.transform;
            }
        }
        
    }



    private void MoveAndShoot()
    {
        if (aiPlayer.targetedEnemy == null)
        {
            //this return happens if enemy dies
            return; //avoid running code we don't need to.
        }

        aiPlayer.navMeshAgent.destination = aiPlayer.targetedEnemy.position;
        float remainingDistance = Vector3.Distance(aiPlayer.targetedEnemy.position, aiPlayer.transform.position);
        if (remainingDistance >= aiPlayer.activeAbility.effectiveRange)
        {
            aiPlayer.navMeshAgent.Resume();
            walking = true;
        }
        else
        {
            //Within range, look at enemy and shoot
            aiPlayer.transform.LookAt(aiPlayer.targetedEnemy);
            //Vector3 dirToShoot = targetedEnemy.transform.position - transform.position; //unused, would be for raycasting

            if (aiPlayer.activeAbility.isReady())
            {
                aiPlayer.activeAbility.Execute(aiPlayer.attributes, aiPlayer.gameObject, aiPlayer.targetedEnemy.gameObject);
                if (!aiPlayer.activeAbility.isbasicAttack)
                {
                    aiPlayer.activeAbility = aiPlayer.abilities.Basic;
                }
            }

            aiPlayer.navMeshAgent.Stop(); //within range, stop moving
            walking = false;
        }


    }
}
