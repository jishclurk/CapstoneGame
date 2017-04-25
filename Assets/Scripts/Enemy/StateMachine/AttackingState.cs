using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This isn't the greatest way to handle attacks, but for testing purposes, its being 
    kept simple until animations are implemented. Should alter attack animation by the 
    attack speed defined in the EnemyAttack object, and should use Unity Animation Events
    to determine when damage calculations should be applied. 
*/

public class AttackingState : IEnemyState {

    public readonly EnemyStateControl enemy;

    public AttackingState(EnemyStateControl stateControl)
    {
        enemy = stateControl;
    }

    public void UpdateState()
    {

        enemy.DisableNavRotation();
        enemy.navMeshAgent.Stop();
        CheckRange();
        AttackTarget();
    }

    public void ToIdleState()
    {
        enemy.EnableNavRotation();
        Debug.Log("Can't transition from attack state to idle state");
    }

    public void ToReturningState()
    {
        enemy.EnableNavRotation();
        enemy.StopTargetting();
        enemy.currentState = enemy.returningState;
    }

    public void ToChasingState()
    {
        enemy.EnableNavRotation();
        enemy.currentState = enemy.chasingState;
    }

    public void ToAttackingState()
    {
        enemy.EnableNavRotation();
        Debug.Log("Can't transition from attack state to attack state");
    }

    private void CheckRange()
    {
        if (Vector3.Distance(enemy.GetTargetPosition(), enemy.transform.position) > (enemy.attack.attackRange - enemy.attack.attackRangeOffset))
        {
            ToChasingState();
        }
    }

    private void AttackTarget()
    {
        // Bad way to make the LookAt() function only rotate around y
        float oldX = enemy.transform.rotation.x;
        float oldZ = enemy.transform.rotation.z;
        enemy.transform.LookAt(enemy.GetTargetPosition(), enemy.transform.up);
        enemy.transform.rotation = new Quaternion(oldX, enemy.transform.rotation.y, oldZ, enemy.transform.rotation.w);

        enemy.animator.AnimateAttack();

        if (enemy.IsTargetDead())
            ToReturningState();
    }
}
