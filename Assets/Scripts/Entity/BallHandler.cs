using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame{

	public class BallHandler : MonoBehaviour {

		public Ball ballPrefabRef;
		private List<Ball> balls;
        public ParticleSystem ballExplode;
        public int numBalls = 1;

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

        //Destroys a single ball with an explosion
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

        //Saves current state of balls
        public List<BallState> saveState()
        {
            List<BallState> savedStates = new List<BallState>();

            foreach(Ball ball in balls)
            {
                BallState ballState = new BallState();
                SerializableVector3 velocity = new SerializableVector3();
                velocity.setVector(ball.GetComponent<Rigidbody>().velocity);
                ballState.velocity = velocity;
                SerializableVector3 position = new SerializableVector3();
                position.setVector(ball.GetPosition());
                ballState.position = position;
                savedStates.Add(ballState);
            }

            return savedStates;
        }

        public void setState(List<BallState> savedState)
        {
            Destroy();

            foreach(BallState savedBall in savedState)
            {
                Ball ballToAdd = Instantiate(ballPrefabRef);
                ballToAdd.spawner = this;
                ballToAdd.SetPosition(savedBall.position.Deserialize());
                Debug.Log(savedBall.velocity.Deserialize());
                ballToAdd.GetComponent<Rigidbody>().velocity = savedBall.velocity.Deserialize();
                balls.Add(ballToAdd);
            }
        }

        public void Destroy()
        {
            if (balls.Count > 0)
            {
                for (int i = 0; i < balls.Count; i++)
                {
                    Destroy(balls[i].gameObject);
                    balls.Remove(balls[i]);
                    i--;
                }
            }

        }

        public void Restart()
        {
            Destroy();
            CreateBalls();
        }
    }

    
}
