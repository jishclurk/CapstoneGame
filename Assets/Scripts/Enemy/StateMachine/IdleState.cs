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
        enemy.sounds.PlayAggroSound();
        enemy.currentState = enemy.chasingState;
    }

    public void ToAttackingState()
    {
        enemy.currentState = enemy.attackingState;
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
                enemy.ChangeTarget(hit.collider.gameObject);
                minDist = testDist;
                ToChasingState();
            }
        }
    }

    private void Idle()
    {
        enemy.navMeshAgent.Stop();
        enemy.animator.AnimateIdle();
    }

}
