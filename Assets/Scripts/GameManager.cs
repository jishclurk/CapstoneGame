using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame {
	
    public class GameManager : MonoBehaviour {

	    private int enemyScore;
	    public int EnemyScore { get { return enemyScore; } set { enemyScore = value; } }
	    private int playerScore;
	    public int PlayerScore { get { return playerScore; } set { playerScore = value; } }

	    public BallHandler bh;
        public Scoreboard scoreboard;

	    private int endScore = 5;

	    // Use this for initialization
	    void Awake(){
            bh.setGameManager(this);
        }

        void Start () {
		    enemyScore = 0;
		    playerScore = 0;
            scoreboard.updateEnemyScore(enemyScore);
            scoreboard.updatePlayerScore(playerScore);
        }
	
	    // Update is called once per frame
	    void Update () {

	    }

        public void ScoreEnemyGoal()
        {
            enemyScore++;
            scoreboard.updateEnemyScore(enemyScore);
        }

        public void ScorePlayerGoal()
        {
            playerScore++;
            scoreboard.updatePlayerScore(playerScore);
        }

    }

}
