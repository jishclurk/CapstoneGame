using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class AttackState : ICoopState
{

    private readonly CoopAiController aiPlayer;
    private float animSpeed;
    public bool reEvalutateTarget;
    private GameObject aoeArea;
    private float timeBetweenCasts;
    private float lastAbilityCast;
    private float lastAbilityDelay;

    public AttackState(CoopAiController statePatternPlayer)
    {
        animSpeed = 0.0f;
        aiPlayer = statePatternPlayer;
        reEvalutateTarget = true;
        switch (aiPlayer.abilityChoose) {
            case Strategy.AbilityPref.Agressive:
                timeBetweenCasts = 1.0f;
                break;
            case Strategy.AbilityPref.Balanced:
                timeBetweenCasts = 5.0f;
                break;
            case Strategy.AbilityPref.Offensive:
            case Strategy.AbilityPref.Defensive:
                timeBetweenCasts = 3.0f;
                break;
            case Strategy.AbilityPref.None:
                timeBetweenCasts = Mathf.Infinity;
                break;
            default:
                timeBetweenCasts = 1.0f;
                break;
        }
        lastAbilityDelay = 0.0f;
        lastAbilityCast = -Mathf.Infinity;
    }

    public void setPrefs()
    {
        switch (aiPlayer.abilityChoose)
        {
            case Strategy.AbilityPref.Agressive:
                timeBetweenCasts = 1.0f;
                break;
            case Strategy.AbilityPref.Balanced:
                timeBetweenCasts = 5.0f;
                break;
            case Strategy.AbilityPref.Offensive:
            case Strategy.AbilityPref.Defensive:
                timeBetweenCasts = 3.0f;
                break;
            case Strategy.AbilityPref.None:
                timeBetweenCasts = Mathf.Infinity;
                break;
            default:
                timeBetweenCasts = 1.0f;
                break;
        }
    }

    public void UpdateState()
    {
        if (aiPlayer.targetedEnemy == null || reEvalutateTarget)
        {
            switch (aiPlayer.targetChoose)
            {
                case Strategy.TargetPref.Closest:
                    FindClosestTarget();
                    break;
                case Strategy.TargetPref.Lowest:
                    FindLowestHealthTarget();
                    break;
                case Strategy.TargetPref.Active:
                    FindActivePlayerTarget();
                    break;
                default:
                    FindClosestTarget();
                    break;
            }
            reEvalutateTarget = false;
            
        }
        if(aiPlayer.targetedEnemy != null){
            if (aiPlayer.targetedEnemy.GetComponent<EnemyHealth>().isDead)
            {
                reEvalutateTarget = true;
            }
            MoveAndShoot();
            if (aiPlayer.activeSpecialAbility == null)
            {
                if(Time.time > lastAbilityCast + timeBetweenCasts + lastAbilityDelay)
                {
                    EvaluateSpecial();
                }
            }
            else
            {
                lastAbilityCast = Time.time;
                //Use Special based on its action
                switch (aiPlayer.activeSpecialAbility.GetCoopAction())
                {
                    case AbilityHelper.CoopAction.TargetHurt:
                        lastAbilityDelay = aiPlayer.activeSpecialAbility.timeToCast;
                        UseOffensiveTargetSpecial();
                        break;
                    case AbilityHelper.CoopAction.AOEHurt:
                        lastAbilityDelay = aiPlayer.activeSpecialAbility.timeToCast;
                        UseOffensiveTargetSpecial();
                        break;
                    case AbilityHelper.CoopAction.Equip:
                        lastAbilityDelay = aiPlayer.activeSpecialAbility.timeToCast;
                        aiPlayer.activeSpecialAbility.Execute(aiPlayer.player, aiPlayer.gameObject, aiPlayer.gameObject);
                        break;
                    case AbilityHelper.CoopAction.InstantHeal:
                        lastAbilityDelay = aiPlayer.activeSpecialAbility.timeToCast;
                        aiPlayer.activeSpecialAbility.Execute(aiPlayer.player, aiPlayer.gameObject, aiPlayer.gameObject);
                        break;
                    case AbilityHelper.CoopAction.AOEHeal:
                        lastAbilityDelay = aiPlayer.activeSpecialAbility.timeToCast;
                        UseDefensiveTargetSpecial();
                        break;
                    default:
                        aiPlayer.activeSpecialAbility = null;
                        break;
                }
                
                aiPlayer.activeSpecialAbility = null;
            }

        }

        aiPlayer.CheckLocalVision(); //Update visible enemies
        WatchActiveplayerForFlee();
        HandleDestinationAnimation(aiPlayer.targetedEnemy, aiPlayer.abilities.Basic);

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
        aiPlayer.navMeshAgent.speed = aiPlayer.navSpeedDefault;
        aiPlayer.currentState = aiPlayer.idleState;
    }

    public void ToCastState()
    {
        aiPlayer.currentState = aiPlayer.castState;
    }

    public void ToFleeState()
    {
        aiPlayer.navMeshAgent.speed = aiPlayer.navSpeedDefault;
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
        bool specialAbilityReady = false;
        int i;
        for (i = 0; i < aiPlayer.abilities.abilityArray.Length && !specialAbilityReady; i++)
        {
            ISpecial potentialAbility = aiPlayer.abilities.abilityArray[i];
            //maybe to "dumbify" ai make it some ratio of time since ready to full cooldown?
            //note: empty ability always returns false on isReady
            if (potentialAbility.isReady() && potentialAbility.energyRequired < aiPlayer.player.resources.currentEnergy && MatchesAbilityPref(potentialAbility))
            {

                //Decide whether to use ability or continue searching (sorry this got kinda complicated!), might want a separate method?

                if (potentialAbility.EvaluateCoopUse(aiPlayer.player, aiPlayer.targetedEnemy, aiPlayer.tm))
                {
                    aiPlayer.activeSpecialAbility = potentialAbility;
                }
                else
                {
                    aiPlayer.activeSpecialAbility = null;
                }

                if(aiPlayer.activeSpecialAbility != null)
                {
                    specialAbilityReady = true;
                }
                
                //Debug.Log("Evaluating ability " + i);

            }
        }
    }

    private bool MatchesAbilityPref(ISpecial ability)
    {
        bool matches = false;
        AbilityHelper.CoopAction act = ability.GetCoopAction();
        if (aiPlayer.abilityChoose == Strategy.AbilityPref.Offensive)
        {
            matches = act == AbilityHelper.CoopAction.AOEHurt || act == AbilityHelper.CoopAction.Equip || act == AbilityHelper.CoopAction.TargetHurt || ability.id == 10; //special case for invigorate

        } else if (aiPlayer.abilityChoose == Strategy.AbilityPref.Defensive)
        {
            matches = act == AbilityHelper.CoopAction.AOEHeal || act == AbilityHelper.CoopAction.Equip || act == AbilityHelper.CoopAction.TargetHeal || act == AbilityHelper.CoopAction.InstantHeal;
        } else
        {
            matches = true;
        }
        return matches;
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

            if(aiPlayer.activeSpecialAbility.GetCoopAction() == AbilityHelper.CoopAction.AOEHurt)
            {
                aoeArea = GameObject.Instantiate(aiPlayer.activeSpecialAbility.aoeTarget, aiPlayer.targetedEnemy.position, Quaternion.Euler(-90, 0, 0)) as GameObject;
                aoeArea.GetComponent<AOETargetController>().isPlayerCalled = false;
                aiPlayer.ah.CoopExecuteAOE(aiPlayer.player, aiPlayer.gameObject, aoeArea, aiPlayer.activeSpecialAbility);
            } else
            {
                aiPlayer.activeSpecialAbility.Execute(aiPlayer.player, aiPlayer.gameObject, aiPlayer.targetedEnemy.gameObject);
            }

            //check if enemy died
            EnemyHealth enemyHP = aiPlayer.targetedEnemy.GetComponent<EnemyHealth>();
            if (enemyHP != null)
            {
                if (enemyHP.isDead)
                {
                    //on kill, remove from both team manager visible enemies and all local watchedenemies
                    aiPlayer.targetedEnemy = null;
                }
            }
            reEvalutateTarget = true;
            aiPlayer.navMeshAgent.Stop(); //within range, stop moving
            animSpeed = 0.0f;
        }
    }


    private void UseDefensiveTargetSpecial()
    {
        if (aiPlayer.activeSpecialAbility.GetCoopAction() == AbilityHelper.CoopAction.AOEHeal)
        {
            aoeArea = GameObject.Instantiate(aiPlayer.activeSpecialAbility.aoeTarget, aiPlayer.transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject;
            aoeArea.GetComponent<AOETargetController>().isPlayerCalled = false;
            aiPlayer.ah.CoopExecuteAOE(aiPlayer.player, aiPlayer.gameObject, aoeArea, aiPlayer.activeSpecialAbility);
        }

        /*if (aiPlayer.targetedEnemy == null)
        {
            //this return happens if enemy dies
            return; //avoid running code we don't need to.
        }

        float remainingDistance = Vector3.Distance(aiPlayer.targetedEnemy.position, aiPlayer.transform.position);
        if (remainingDistance <= aiPlayer.activeSpecialAbility.effectiveRange && aiPlayer.isTargetVisible(aiPlayer.targetedEnemy))
        {
            aiPlayer.transform.LookAt(aiPlayer.targetedEnemy);

            if (aiPlayer.activeSpecialAbility.GetCoopAction() == AbilityHelper.CoopAction.AOEHurt)
            {
                aoeArea = GameObject.Instantiate(aiPlayer.activeSpecialAbility.aoeTarget, aiPlayer.targetedEnemy.position, Quaternion.Euler(-90, 0, 0)) as GameObject;
                aoeArea.GetComponent<AOETargetController>().isPlayerCalled = false;
                aiPlayer.ah.CoopExecuteAOE(aiPlayer.player, aiPlayer.gameObject, aoeArea, aiPlayer.activeSpecialAbility);
            }
            else
            {
                aiPlayer.activeSpecialAbility.Execute(aiPlayer.player, aiPlayer.gameObject, aiPlayer.targetedEnemy.gameObject);
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
            reEvalutateTarget = true;
            aiPlayer.navMeshAgent.Stop(); //within range, stop moving
            animSpeed = 0.0f;
        }
        */
    }

    private void MoveAndShoot()
    {
        if (aiPlayer.targetedEnemy == null)
        {
            //this return happens if enemy dies
            return; //avoid running code we don't need to.
        }

        float remainingDistance = Vector3.Distance(aiPlayer.targetedEnemy.position, aiPlayer.transform.position);
        if (remainingDistance <= aiPlayer.abilities.Basic.effectiveRange && aiPlayer.isTargetVisible(aiPlayer.targetedEnemy))
        {
            //Within range, look at enemy and shoot
            aiPlayer.transform.LookAt(aiPlayer.targetedEnemy);

            if (aiPlayer.abilities.Basic.isReady() && Time.time > lastAbilityCast + lastAbilityDelay) //EQUIP abilities are checked here.
            {
                lastAbilityDelay = 0.0f;
                aiPlayer.animController.AnimateShoot();
                aiPlayer.abilities.Basic.Execute(aiPlayer.player, aiPlayer.gameObject, aiPlayer.targetedEnemy.gameObject);

                //check if enemy died
                EnemyHealth enemyHP = aiPlayer.targetedEnemy.GetComponent<EnemyHealth>();
                if (enemyHP != null)
                {
                    if (enemyHP.isDead)
                    {
                        //on kill, remove from both team manager visible enemies and all local watchedenemies
                        aiPlayer.targetedEnemy = null;
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
