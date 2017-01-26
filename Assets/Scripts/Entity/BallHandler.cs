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
            ParticleSystem explode = Instantiate(ballExplode, ball.gameObject.transform.position, ball.gameObject.transform.rotation);
            Destroy(explode.gameObject, 5.0f);
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

        public List<BallState> saveState()
        {
            List<BallState> savedStates = new List<BallState>();

            foreach(Ball ball in balls)
            {
                savedStates.Add(new BallState(ball.velocity, ball.GetPosition()));
            }

            return savedStates;
        }

        public void setState(List<BallState> savedState)
        {
            foreach(Ball ball in balls)
            {
                Destroy(ball);
            }

            foreach(BallState savedBall in savedState)
            {
                Ball ballToAdd = Instantiate(ballPrefabRef);
                ballToAdd.spawner = this;
                ballToAdd.SetPosition(savedBall.position);
                ballToAdd.velocity = savedBall.velocity;
                balls.Add(ballToAdd);
            }
        }
    }

    
}
