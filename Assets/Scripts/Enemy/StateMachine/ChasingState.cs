using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingState : IEnemyState {

    private readonly EnemyStateControl enemy;

    public ChasingState(EnemyStateControl stateControl)
    {
        enemy = stateControl;
    }

    public void UpdateState()
    {
        Look();
        Chase();
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void ToIdleState()
    {
        Debug.Log("Should not transition from chasing to idle states");
    }

    public void ToReturningState()
    {
        enemy.currentState = enemy.returningState;
    }

    public void ToChaseState()
    {
        Debug.Log("Can't transition from chase state to chase state");
    }

    public void ToAttackingState()
    {
        enemy.currentState = enemy.attackingState;
    }

    private void Look()
    {
        RaycastHit hit;
        Vector3 enemyToTarget = enemy.chaseTarget.position - enemy.transform.position;
        if (Physics.Raycast(enemy.transform.position, enemyToTarget, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))
        {
            enemy.chaseTarget = hit.transform;
        }
        else
        {
            // Change This?
            ToReturningState();
        }
    }

    private void Chase()
    {
        enemy.meshRendererFlag.material.color = Color.red;
        enemy.navMeshAgent.destination = enemy.chaseTarget.position;
        enemy.navMeshAgent.Resume();
    }

}
