using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame{

	public class BallHandler : MonoBehaviour {

		public Ball ballPrefabRef;
		private List<Ball> balls;
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
			balls = new List<Ball> ();
            CreateBalls();
            enemyScore = gameObject.GetComponents<AudioSource>()[0];
            playerScore = gameObject.GetComponents<AudioSource>()[1];
        }

        private void CreateBalls() {
            for (int i = 0; i < numBalls; i++)
            {
				balls.Add( (Ball)Instantiate(ballPrefabRef));
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

    }
}
