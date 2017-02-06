using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ReturningState : IEnemyState {

    public readonly EnemyStateControl enemy;

    public ReturningState(EnemyStateControl stateControl)
    {
        enemy = stateControl;
    }

    public void UpdateState()
    {
        CheckVision();
        ReturnToSpawn();
    }

    public void ToIdleState()
    {
        enemy.currentState = enemy.idleState;
    }

    public void ToReturningState()
    {
        Debug.Log("Can't transition between returning state and returning state");
    }

    public void ToChasingState()
    {
        enemy.currentState = enemy.chasingState;
    }

    public void ToAttackingState()
    {
        Debug.Log("Can't transition between returning state and attack state");
    }

    private void CheckVision()
    {
        foreach (GameObject player in enemy.visiblePlayers)
        {
            RaycastHit hit;
            Vector3 enemyToTarget = player.transform.position - enemy.transform.position;
            if (Physics.Raycast(enemy.transform.position, enemyToTarget, out hit) && hit.collider.CompareTag("Player"))
            {
                enemy.chaseTarget = hit.collider.gameObject;
                ToChasingState();
                return;
            }
        }
    }

    private void ReturnToSpawn()
    {
        enemy.meshRendererFlag.material.color = Color.blue;
        enemy.navMeshAgent.destination = enemy.returnPosition;
        enemy.navMeshAgent.Resume();
        enemy.animator.AnimateMovement();

        // Have to manually do this because using pathStatus doesn't work
        if (enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance)
        {
            ToIdleState();
        }
    }
}
