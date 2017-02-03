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
    private EnemyAttack attack;
    private float timer;

    public AttackingState(EnemyStateControl stateControl)
    {
        enemy = stateControl;
        attack = enemy.gameObject.GetComponent<EnemyAttack>();
        timer = 0;
    }

    public void UpdateState()
    {
        enemy.meshRendererFlag.material.color = Color.black;
        timer += Time.fixedDeltaTime;
        CheckRange();
        AttackTarget();
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void ToIdleState()
    {
        timer = 0;
        Debug.Log("Can't transition from attack state to idle state");
    }

    public void ToReturningState()
    {
        timer = 0;
        enemy.currentState = enemy.returningState;
    }

    public void ToChasingState()
    {
        timer = 0;
        enemy.currentState = enemy.chasingState;
    }

    public void ToAttackingState()
    {
        timer = 0;
        Debug.Log("Can't transition from attack state to attack state");
    }

    private void CheckRange()
    {
        if (Vector3.Distance(enemy.chaseTarget.position, enemy.transform.position) > (attack.attackRange - attack.attackRangeOffset))
        {
            ToChasingState();
        }

    }

    private void AttackTarget()
    {
        if (timer >= attack.attackSpeed)
        {
            timer = 0;
            PlayerHealth playerHP = enemy.chaseTarget.GetComponent<PlayerHealth>();
            if (playerHP != null)
            {
                playerHP.TakeDamage(attack.attackDamage);
                if (playerHP.isDead)
                    ToReturningState();
            }
        }
    }
}
