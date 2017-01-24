using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame{


	public class BallHandler : MonoBehaviour {

		public Ball ballPrefabRef;
		public List<Ball> balls = new List<Ball>();
        public ParticleSystem ballExplode;
        public int numBalls = 1;

        private GameManager gameManager;
        private AudioSource enemyScore;
        private AudioSource playerScore;

		//public int StartBallNumber { get { return numBalls; } set  {numBalls = value;}}
		public int BallCount { get { return balls.Count; }  }

        public void setGameManager(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        //???
		//[Range(1.0010f,1.0100f)]
		//public float speedUp = 1.0010f;

		void Start(){
            CreateBalls();
            enemyScore = gameObject.GetComponents<AudioSource>()[0];
            playerScore = gameObject.GetComponents<AudioSource>()[1];
        }

        private void CreateBalls() {
            for (int i = 0; i < numBalls; i++)
            {
                balls[i] = (Ball)Instantiate(ballPrefabRef);
            }
        }

		//Removes Balls that have been missed, creates new balls
		void FixedUpdate () {

            foreach (Ball ball in balls)
            {
                if (ball.hitEnemy)
                {
                    enemyScore.Play();
                    Instantiate(ballExplode, ball.gameObject.transform.position, ball.gameObject.transform.rotation);
                    gameManager.EnemyScore++;
                    Destroy(ball);
                    balls.Remove(ball);
                    //distroy ball, add score, if balls = zero
                }
                else if (ball.hitPlayer)
                {
                    playerScore.Play();
                    Instantiate(ballExplode, ball.gameObject.transform.position, ball.gameObject.transform.rotation);
                    gameManager.PlayerScore++;
                    Destroy(ball);
                    balls.Remove(ball);
                }
            }

            if(balls.Count == 0)
            {
                CreateBalls();
            }

				
		}

		//public Ball GetUnWatchedBall(){
		//	for (int i=0; i<balls.Count; i++){
		//		if (!balls [i].BeingWatched ()) {
		//			//balls [i].SetWatched (true);
		//			return balls [i];
		//		}
		//	}
		//	return null;
		//}

		//public void addBalls(){
		//	for (int i = 0; i < numBalls; i++) {
  //              balls[i] = (Ball)Instantiate(ballPrefabRef);
		//	}
		//}

	//	void OnGUI(){
	//		GUI.TextArea(new Rect (0,0,100,50),"Enemy Score   "+eScore);
	//		GUI.TextArea(new Rect (Screen.width-100,0,100,50),"PLayer Score   "+pScore);
	//
	//		if (pGoal && !endCondition) {
	//			if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height / 2 + 100, 200, 100), "Start New Round")) {
	//				rb.velocity = new Vector3 (5, 3, 0);\
	//				pGoal = false;
	//			}
	//		}
	//
	//		if (eGoal && !endCondition) {
	//			if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height / 2 + 100, 200, 100), "Start New Round")) {
	//				rb.velocity = new Vector3 (5, 3, 0);
	//				eGoal = false;
	//			}
	//		}
	//
	//		if (endCondition) {
	//			if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height / 2 + 100, 200, 100), "Quit?")) {
	//				Application.Quit ();
	//			}
	//			if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "Game Over, New game?")) {
	//				SceneManager.LoadScene("Pong");
	//			}
	//		}
	//	}
		}
}
