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

	    private int endScore = 5;

	    // Use this for initialization
	    void Awake(){
            bh.setGameManager(this);
        }

        void Start () {
		    enemyScore = 0;
		    playerScore = 0;
	    }
	
	    // Update is called once per frame
	    void Update () {

	    }

        public void ScoreEnemyGoal()
        {
            enemyScore++;
        }

        public void ScorePlayerGoal()
        {
            playerScore++;
        }

        void OnGUI(){
		    GUI.TextArea(new Rect (Screen.width-100,0,100,50),"Player Score   "+playerScore);
		    GUI.TextArea(new Rect (0,0,100,50),"Enemy Score   "+enemyScore);
	    }

    }

}
