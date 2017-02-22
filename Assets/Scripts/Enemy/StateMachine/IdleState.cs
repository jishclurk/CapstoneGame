using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LayerDefinitions;

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
        //enemy.sounds.PlayAggroSound();
        enemy.currentState = enemy.chasingState;
    }

    public void ToAttackingState()
    {
        enemy.currentState = enemy.attackingState;
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

    private void Idle()
    {
        enemy.navMeshAgent.Stop();
        enemy.animator.AnimateIdle();
    }

}
