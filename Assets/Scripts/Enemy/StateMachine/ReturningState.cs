using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturningState : IEnemyState {

    public readonly EnemyStateControl enemy;

    public ReturningState(EnemyStateControl stateControl)
    {
        enemy = stateControl;
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
