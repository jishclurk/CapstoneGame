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
        ReturnToSpawn();
    }

    // This is really bad, not going to work for a lot of situations, or switching between multiple player targets
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RaycastHit hit;
            Vector3 enemyToTarget = other.transform.position - enemy.transform.position;
            if (Physics.Raycast(enemy.transform.position, enemyToTarget, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))
            {
                enemy.chaseTarget = hit.transform;
                ToChasingState();
            }
        }
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

    private void ReturnToSpawn()
    {
        enemy.meshRendererFlag.material.color = Color.blue;
        enemy.navMeshAgent.destination = enemy.returnLocation.position;
        enemy.navMeshAgent.Resume();

        // Have to manually do this because using pathStatus doesn't work
        if (enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance)
        {
            ToIdleState();
        }
    }
}
