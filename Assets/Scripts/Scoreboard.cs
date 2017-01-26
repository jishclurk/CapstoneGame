using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour {

    public Text playerScore;
    public Text enemyScore;

    public void updateEnemyScore(int newScore)
    {
        if (enemyScore != null)
            enemyScore.text = newScore.ToString();
    }

    public void updatePlayerScore(int newScore)
    {
        if (playerScore != null)
            playerScore.text = newScore.ToString();
    }

    public void updateAll(int enemyScore, int playerScore)
    {
        this.enemyScore.text = enemyScore.ToString();
        this.enemyScore.text = playerScore.ToString();
    }
}
