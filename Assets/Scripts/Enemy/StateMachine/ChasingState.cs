using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LayerDefinitions;

public class ChasingState : IEnemyState {

    private readonly EnemyStateControl enemy;

    public ChasingState(EnemyStateControl stateControl)
    {
        enemy = stateControl;
    }

    public void UpdateState()
    {
        //CheckSpawnDistance();
        Look();
        Chase();
    }

    public void ToIdleState()
    {
        Debug.Log("Should not transition from chasing to idle states");
    }

    public void ToReturningState()
    {
        enemy.StopTargetting();
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
            enemy.ReportTargetOutOfRange();
            ToReturningState();
        }
    }

    private void Look()
    {
        RaycastHit hit;
        Vector3 enemyToTarget = new Vector3(enemy.GetTargetPosition().x - enemy.eyes.position.x, 0, enemy.GetTargetPosition().z - enemy.eyes.position.z);
        if (Physics.Raycast(enemy.eyes.position, enemyToTarget, out hit, 100f, Layers.NonEnemy) && hit.collider.gameObject.CompareTag("Player"))
        {
            if (Vector3.Distance(enemy.GetTargetPosition(), enemy.transform.position) <= (enemy.attack.attackRange - enemy.attack.attackRangeOffset))
            {
                ToAttackingState();
                return;
            }
        }
        else
        {
            // If chasing target is not in our vision, check if another player is
            foreach (GameObject player in enemy.GetVisiblePlayers())
            {
                enemyToTarget = new Vector3(player.transform.position.x - enemy.eyes.position.x, 0, player.transform.position.z - enemy.eyes.position.z);
                if (Physics.Raycast(enemy.eyes.position, enemyToTarget, out hit, 100f, Layers.NonEnemy) && hit.collider.gameObject.CompareTag("Player"))
                {
                    enemy.FindTarget();
                    return;
                }
            }

            // If no players visible, return to spawn
            ToReturningState();
        }
    }

    private void Chase()
    {
        enemy.navMeshAgent.destination = enemy.GetTargetPosition();
        enemy.navMeshAgent.Resume();
        enemy.animator.AnimateMovement();
    }

}
