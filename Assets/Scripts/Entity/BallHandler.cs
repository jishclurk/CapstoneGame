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

		void Start(){
			balls = new List<Ball> ();
            CreateBalls();
        }

        private void CreateBalls() {
            for (int i = 0; i < numBalls; i++)
            {
                Ball ballToAdd = Instantiate(ballPrefabRef);
                ballToAdd.spawner = this;
				balls.Add(ballToAdd);
            }
        }

        public void DestroyBall(Ball ball)
        {
            Instantiate(ballExplode, ball.gameObject.transform.position, ball.gameObject.transform.rotation);
            balls.Remove(ball);
            Destroy(ball.gameObject);
        }

		//Removes Balls that have been missed, creates new balls
		void FixedUpdate () {

            if(balls.Count == 0)
            {
                CreateBalls();
            }
		}



    }
}
