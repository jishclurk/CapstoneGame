using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using LayerDefinitions;

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
        enemy.sounds.PlayAggroSound();
        enemy.currentState = enemy.chasingState;
    }

    public void ToAttackingState()
    {
        Debug.Log("Can't transition between returning state and attack state");
    }

    private void CheckVision()
    {
        foreach (GameObject player in enemy.GetVisiblePlayers())
        {
            RaycastHit hit;
            Vector3 enemyToTarget = new Vector3(player.transform.position.x - enemy.eyes.position.x, 0, player.transform.position.z - enemy.eyes.position.z);
            if (Physics.Raycast(enemy.eyes.position, enemyToTarget, out hit, 100f, Layers.NonEnemy) && hit.collider.gameObject.CompareTag("Player"))
            {
                enemy.FindTarget();
                ToChasingState();
                return;
            }
        }
    }

    private void ReturnToSpawn()
    {
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
