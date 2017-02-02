using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingState : IEnemyState {

    public readonly EnemyStateControl enemy;
    private EnemyAttack attack;

    public AttackingState(EnemyStateControl stateControl)
    {
        enemy = stateControl;
        attack = enemy.gameObject.GetComponent<EnemyAttack>();
    }

    public void UpdateState()
    {

    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void ToIdleState()
    {

    }

    public void ToReturningState()
    {

    }

    public void ToChaseState()
    {

    }

    public void ToAttackingState()
    {

    }
}
