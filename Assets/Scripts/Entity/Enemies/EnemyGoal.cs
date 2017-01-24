using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CapstoneGame;

public class EnemyGoal : MonoBehaviour, IGoal {

    public GameManager manager;

	public void ScoreGoal()
    {
        manager.ScoreEnemyGoal();
    }

}