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

    public void updateAll(int eScore, int pScore)
    {
        enemyScore.text = eScore.ToString();
        playerScore.text = pScore.ToString();
    }

    public void clearAll()
    {
        enemyScore.text = "";
        playerScore.text = "";
    }
}
