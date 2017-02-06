using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : IEnemyState {

    private readonly EnemyStateControl enemy;

    public EnemyIdleState (EnemyStateControl stateControl)
    {
        enemy = stateControl;
    }

    public void UpdateState()
    {
        CheckVision();
        Idle();
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

    private void Idle()
    {
        enemy.navMeshAgent.Stop();
        enemy.meshRendererFlag.material.color = Color.green;
        enemy.animator.AnimateIdle();
    }

}
