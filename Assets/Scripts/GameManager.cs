using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame{
	
public class GameManager : MonoBehaviour {

	private int enemyScore;
	public int EnemyScore { get { return enemyScore; } set { enemyScore = value; } }
	private int playerScore;
	public int PlayerScore { get { return playerScore; } set { playerScore = value; } }

	public BallHandler bh;
	public EnemyHandler eh;

	[Range(1,3)]
	public int numBalls = 3;
	private int numEnemies = 1;
	private int endScore = 5;
	// Use this for initialization
		void Awake(){
			bh.StartBallNumber = numBalls;
			bh.addBalls ();
			eh.EnemyNumber = numEnemies;
		}

	void Start () {


		enemyScore = 0;
		playerScore = 0;
	}
	
	// Update is called once per frame
	void Update () {
			if (PlayerScore == endScore) {

			} else if (EnemyScore == endScore) {

			} else if (bh.BallCount == 0) {
				bh.addBalls ();
				Debug.Log ("Got here");
			}
	}
		
	void OnGUI(){

		GUI.TextArea(new Rect (Screen.width-100,0,100,50),"Player Score   "+playerScore);
		GUI.TextArea(new Rect (0,0,100,50),"Enemy Score   "+enemyScore);
	}



}

}
