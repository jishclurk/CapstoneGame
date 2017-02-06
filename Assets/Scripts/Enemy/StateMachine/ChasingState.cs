using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingState : IEnemyState {

    private readonly EnemyStateControl enemy;
    private EnemyAttack attack;

    public ChasingState(EnemyStateControl stateControl)
    {
        enemy = stateControl;
        attack = enemy.gameObject.GetComponent<EnemyAttack>();
    }

    public void UpdateState()
    {
        CheckSpawnDistance();
        Look();
        Chase();
    }

    public void ToIdleState()
    {
        Debug.Log("Should not transition from chasing to idle states");
    }

    public void ToReturningState()
    {
        enemy.currentState = enemy.returningState;
    }

    public void ToChasingState()
    {
        Debug.Log("Can't transition from chase state to chase state");
    }

    public void ToAttackingState()
    {
        enemy.currentState = enemy.attackingState;
    }

    private void CheckSpawnDistance()
    {
        if (Vector3.Distance(enemy.transform.position, enemy.returnPosition) >= enemy.deaggroDistance)
        {
            ToReturningState();
        }
    }

    private void Look()
    {
        RaycastHit hit;
        Vector3 enemyToTarget = enemy.chaseTarget.transform.position - enemy.transform.position;
        if (Physics.Raycast(enemy.transform.position, enemyToTarget, out hit) && hit.collider.CompareTag("Player"))
        {
            enemy.chaseTarget = hit.collider.gameObject;
            if (Vector3.Distance(enemy.chaseTarget.transform.position, enemy.transform.position) <= (attack.attackRange - attack.attackRangeOffset))
            {
                ToAttackingState();
                return;
            }
        }
        else
        {
            // If chasing target is not in our vision, check if another player is
            foreach (GameObject player in enemy.visiblePlayers)
            {
                enemyToTarget = player.transform.position - enemy.transform.position;
                if (Physics.Raycast(enemy.transform.position, enemyToTarget, out hit) && hit.collider.CompareTag("Player"))
                {
                    enemy.chaseTarget = hit.collider.gameObject;
                    return;
                }
            }

            // If no players visible, return to spawn
            ToReturningState();
        }
    }

    private void Chase()
    {
        enemy.meshRendererFlag.material.color = Color.red;
        enemy.navMeshAgent.destination = enemy.chaseTarget.transform.position;
        enemy.navMeshAgent.Resume();
        enemy.animator.AnimateMovement();
    }

}
