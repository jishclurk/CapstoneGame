using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class AttackState : ICoopState
{

    private readonly CoopAiController aiPlayer;
    private float animSpeed;
    private bool reEvalutateTarget;

    public AttackState(CoopAiController statePatternPlayer)
    {
        animSpeed = 0.0f;
        aiPlayer = statePatternPlayer;
        reEvalutateTarget = true;
    }

    public void UpdateState()
    {
        if (aiPlayer.targetedEnemy == null || reEvalutateTarget)
        {
            switch (aiPlayer.targetChoose)
            {
                case CoopAiController.TargetPref.Closest:
                    FindClosestTarget();
                    break;
                case CoopAiController.TargetPref.Lowest:
                    FindLowestHealthTarget();
                    break;
                case CoopAiController.TargetPref.Active:
                    FindActivePlayerTarget();
                    break;
                default:
                    FindClosestTarget();
                    break;
            }
            reEvalutateTarget = false;
            
        } else
        {
            aiPlayer.CheckLocalVision(); //Update visible enemies
            MoveAndShoot();
            EvaluateSpecial();
            if (aiPlayer.activeSpecialAbility != null)
            {
                Debug.Log("USING: aiPlayer.activeSpecialAbility");
                //Use Special based on its action
                switch (aiPlayer.activeSpecialAbility.GetCoopAction())
                {
                    case AbilityHelper.CoopAction.TargetHurt:
                        UseOffensiveTargetSpecial();
                        break;
                    case AbilityHelper.CoopAction.AOEHurt:
                        UseOffensiveTargetSpecial();
                        break;
                    case AbilityHelper.CoopAction.InstantHeal:
                        aiPlayer.activeSpecialAbility.Execute(aiPlayer.player, aiPlayer.gameObject, aiPlayer.gameObject);
                        break;
                    default:
                        aiPlayer.activeSpecialAbility = null;
                        break;
                }

            }
        }

        WatchActiveplayerForFlee();
        HandleDestinationAnimation(aiPlayer.targetedEnemy, aiPlayer.activeBasicAbility);

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

    private void FindClosestTarget()
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


    private void FindLowestHealthTarget()
    {
        float lowestHealth = float.PositiveInfinity;
        GameObject bestTarget = null;

        foreach (GameObject enemy in aiPlayer.tm.visibleEnemies)
        {
            float enemyHealth = enemy.GetComponent<EnemyHealth>().currentHealth;
            if (enemyHealth < lowestHealth)
            {
                lowestHealth = enemyHealth;
                bestTarget = enemy;
            }
        }

        if (bestTarget == null)
        {
            ToIdleState();
        }
        else
        {
            aiPlayer.targetedEnemy = bestTarget.transform;
        }

    }

    private void FindActivePlayerTarget()
    {
        aiPlayer.targetedEnemy = aiPlayer.tm.activePlayer.GetComponent<PlayerController>().targetedEnemy;
        if (aiPlayer.targetedEnemy == null)
        {
            FindClosestTarget();
        }

    }

    //sets aiPlayer.activeSpecialAbility
    private void EvaluateSpecial()
    {
        aiPlayer.activeSpecialAbility = null;
        bool specialAbilityReady = false;
        int i;
        for (i = 0; i < aiPlayer.abilities.abilityArray.Length && !specialAbilityReady; i++)
        {
            ISpecial potentialAbility = aiPlayer.abilities.abilityArray[i];
            //maybe to "dumbify" ai make it some ratio of time since ready to full cooldown?
            //note: empty ability always returns false on isReady
            if (potentialAbility.isReady() && potentialAbility.energyRequired < aiPlayer.player.resources.currentEnergy)
            {

                //Decide whether to use ability or continue searching (sorry this got kinda complicated!), might want a separate method?
                switch (potentialAbility.GetCoopAction())
                {
                   
                    case AbilityHelper.CoopAction.TargetHurt:
                        if (aiPlayer.targetedEnemy != null)
                        { 
                            aiPlayer.activeSpecialAbility = potentialAbility;
                        }
                        break;
                    case AbilityHelper.CoopAction.AOEHurt:
                        //aiPlayer.activeSpecialAbility = potentialAbility;
                        //UseOffensiveSpecialTarget does not quite work for AOE. Needs to instantiate the AOE cicle in order to know what enemies to hit. NC will work on it soon
                        break;
                    case AbilityHelper.CoopAction.InstantHeal:
                        if(aiPlayer.player.resources.maxHealth - aiPlayer.player.resources.currentHealth >= potentialAbility.baseDamage)
                        { //using abilities in IDLE state ???? probably not. Up to player at that point.
                            aiPlayer.activeSpecialAbility = potentialAbility;
                        }
                        break;
                    default:
                        aiPlayer.activeSpecialAbility = null;
                        break;
                }
                if(aiPlayer.activeSpecialAbility != null)
                {
                    specialAbilityReady = true;
                }
                
                //Debug.Log("Evaluating ability " + i);

            }
        }
    }

    private void UseOffensiveTargetSpecial()
    {
        if (aiPlayer.targetedEnemy == null)
        {
            //this return happens if enemy dies
            return; //avoid running code we don't need to.
        }

        float remainingDistance = Vector3.Distance(aiPlayer.targetedEnemy.position, aiPlayer.transform.position);
        if (remainingDistance <= aiPlayer.activeSpecialAbility.effectiveRange && aiPlayer.isTargetVisible(aiPlayer.targetedEnemy))
        {
            aiPlayer.transform.LookAt(aiPlayer.targetedEnemy);

            aiPlayer.activeSpecialAbility.Execute(aiPlayer.player, aiPlayer.gameObject, aiPlayer.targetedEnemy.gameObject);
            aiPlayer.activeSpecialAbility = null;

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
            reEvalutateTarget = true;
            aiPlayer.navMeshAgent.Stop(); //within range, stop moving
            animSpeed = 0.0f;
        }

    }

    private void MoveAndShoot()
    {
        if (aiPlayer.targetedEnemy == null)
        {
            //this return happens if enemy dies
            return; //avoid running code we don't need to.
        }



        float remainingDistance = Vector3.Distance(aiPlayer.targetedEnemy.position, aiPlayer.transform.position);
        if (remainingDistance <= aiPlayer.activeBasicAbility.effectiveRange && aiPlayer.isTargetVisible(aiPlayer.targetedEnemy))
        {
            //Within range, look at enemy and shoot
            aiPlayer.transform.LookAt(aiPlayer.targetedEnemy);

            
            if (aiPlayer.activeBasicAbility.isReady())
            {
                aiPlayer.animController.AnimateShoot();
                aiPlayer.activeBasicAbility.Execute(aiPlayer.player, aiPlayer.gameObject, aiPlayer.targetedEnemy.gameObject);

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
                reEvalutateTarget = true;
            }

            aiPlayer.navMeshAgent.Stop(); //within range, stop moving
            animSpeed = 0.0f;
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


    private void HandleDestinationAnimation(Transform target, IAbility ability)
    {
        if (target != null)
        {
            aiPlayer.navMeshAgent.destination = target.position;
            float remainingDistance = Vector3.Distance(target.position, aiPlayer.transform.position);
            if (remainingDistance >= ability.effectiveRange)
            {
                aiPlayer.animController.AnimateAimChasing();
                aiPlayer.navMeshAgent.Resume();
                aiPlayer.navMeshAgent.speed = aiPlayer.navSpeedDefault;
            }
            else if (remainingDistance >= ability.effectiveRange - aiPlayer.chaseEpsilon) 
            {
                aiPlayer.animController.AnimateAimChasing();
                aiPlayer.navMeshAgent.speed = target.GetComponent<NavMeshAgent>().speed;
                aiPlayer.navMeshAgent.Resume();
            }
            else
            {
                aiPlayer.animController.AnimateAimStanding();
                aiPlayer.navMeshAgent.speed = aiPlayer.navSpeedDefault;
                aiPlayer.navMeshAgent.Stop();
            }
        }
        else
        {
            if (aiPlayer.navMeshAgent.remainingDistance > aiPlayer.navMeshAgent.stoppingDistance)
                aiPlayer.animController.AnimateMovement(animSpeed);
            else
                aiPlayer.animController.AnimateIdle();
                
        }

    }

}
