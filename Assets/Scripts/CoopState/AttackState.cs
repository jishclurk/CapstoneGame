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
        if (aiPlayer.targetedEnemy == null)
        {
            FindClosestEnemy();
        } else
        {
            MoveAndShoot();
        }
        WatchActiveplayerForFlee();
        aiPlayer.anim.SetBool("Idling", !walking);
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
        aiPlayer.currentState = aiPlayer.fleeState;
    }

    private void FindClosestEnemy()
    {
        float bestDist = float.PositiveInfinity;
        GameObject bestTarget = null;

        foreach(GameObject enemy in aiPlayer.tm.visibleEnemies)
        {
            Vector3 option = enemy.transform.position - aiPlayer.transform.position;
            if(Vector3.SqrMagnitude(option) < bestDist)
            {
                bestDist = Vector3.SqrMagnitude(option);
                bestTarget = enemy;
            }
        }
           
        if(bestTarget == null){
            ToIdleState();
        } else {
            aiPlayer.targetedEnemy = bestTarget.transform;
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
        if (remainingDistance >= aiPlayer.activeAbility.effectiveRange || !aiPlayer.isTargetVisible(aiPlayer.targetedEnemy))
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

            //check if enemy died
            EnemyHealth enemyHP = aiPlayer.targetedEnemy.GetComponent<EnemyHealth>();
            if (enemyHP != null)
            {
                if (enemyHP.isDead)
                {
                    //on kill, remove from both team manager visible enemies and all local watchedenemies
                    aiPlayer.tm.RemoveDeadEnemy(aiPlayer.targetedEnemy.gameObject);
                    aiPlayer.targetedEnemy = null;
                    if (!aiPlayer.tm.IsTeamInCombat())
                    {
                        ToIdleState();
                    }
                }
            }

            aiPlayer.navMeshAgent.Stop(); //within range, stop moving
            walking = false;
        }
    }



    private void WatchActiveplayerForFlee()
    {
        if(aiPlayer.tm.activePlayer.watchedEnemies.Count == 0)
        {
            aiPlayer.targetedEnemy = null;
            ToFleeState();
        }
    }
}
