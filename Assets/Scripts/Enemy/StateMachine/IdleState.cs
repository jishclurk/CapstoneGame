using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState {

    private readonly EnemyStateControl enemy;

    public IdleState (EnemyStateControl stateControl)
    {
        enemy = stateControl;
    }

    public void UpdateState()
    {
        Idle();
    }

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
        Debug.Log("Enemy can't transition from idle state to idle state");
    }

    public void ToReturningState()
    {
        enemy.currentState = enemy.returningState;
    }

    public void ToChasingState()
    {
        enemy.currentState = enemy.chasingState;
    }

    public void ToAttackingState()
    {
        enemy.currentState = enemy.attackingState;
    }

    private void Idle()
    {
        enemy.meshRendererFlag.material.color = Color.green;
        // Handle Idle Animation
    }

}
