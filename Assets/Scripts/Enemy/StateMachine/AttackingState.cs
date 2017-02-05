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

        enemy.DisableNavRotation();
        enemy.navMeshAgent.Stop();
        CheckRange();
        AttackTarget();
    }

    public void ToIdleState()
    {
        timer = 0;
        enemy.EnableNavRotation();
        Debug.Log("Can't transition from attack state to idle state");
    }

    public void ToReturningState()
    {
        timer = 0;
        enemy.EnableNavRotation();
        enemy.currentState = enemy.returningState;
    }

    public void ToChasingState()
    {
        timer = 0;
        enemy.EnableNavRotation();
        enemy.currentState = enemy.chasingState;
    }

    public void ToAttackingState()
    {
        timer = 0;
        enemy.EnableNavRotation();
        Debug.Log("Can't transition from attack state to attack state");
    }

    private void CheckRange()
    {
        if (Vector3.Distance(enemy.chaseTarget.transform.position, enemy.transform.position) > (attack.attackRange - attack.attackRangeOffset))
        {
            ToChasingState();
        }
    }

    private void AttackTarget()
    {
        // Bad way to make the LookAt() function only rotate around y
        float oldX = enemy.transform.rotation.x;
        float oldZ = enemy.transform.rotation.z;
        enemy.transform.LookAt(enemy.chaseTarget.transform, enemy.transform.up);
        enemy.transform.rotation = new Quaternion(oldX, enemy.transform.rotation.y, oldZ, enemy.transform.rotation.w);


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
