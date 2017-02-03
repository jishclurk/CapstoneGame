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

    public void ToChasingState()
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
            if (Vector3.Distance(enemy.chaseTarget.position, enemy.transform.position) <= (attack.attackRange - attack.attackRangeOffset))
            {
                enemy.navMeshAgent.Stop();
                ToAttackingState();
            }
        }
        else
        {
            // Change This? Shouldn't immediately return when player out of sight
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
