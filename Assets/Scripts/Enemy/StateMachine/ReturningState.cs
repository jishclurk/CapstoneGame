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
        float minDist = float.PositiveInfinity;
        float testDist = 0;
        foreach (GameObject player in enemy.visiblePlayers)
        {
            RaycastHit hit;
            Vector3 enemyToTarget = new Vector3(player.transform.position.x - enemy.eyes.position.x, 0, player.transform.position.z - enemy.eyes.position.z);
            testDist = Vector3.Magnitude(enemyToTarget);
            if (Physics.Raycast(enemy.eyes.position, enemyToTarget, out hit) && hit.collider.gameObject.CompareTag("Player") && testDist < minDist)
            {
                enemy.chaseTarget = hit.collider.gameObject;
                minDist = testDist;
                ToChasingState();
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
