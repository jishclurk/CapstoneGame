using System;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame
{
	public class BallPhysics : IPhysics
	{




		private Vector3 velocity;
		private Rigidbody rb;
		private float speed;

		public BallPhysics (Rigidbody rb){
			this.rb = rb;
		}

		public void UpdateSpeed(float speed){
			this.speed = speed;
		}

		public void UpdateVelocity(Vector3 velocity){
			rb.velocity = rb.velocity * speed;

			if (rb.velocity.x < 2f && rb.velocity.x > -2.0f) {
				float newX = rb.velocity.x * 1.5f;
				rb.velocity = new Vector3 (newX, rb.velocity.y, rb.velocity.z);
			}

			if (rb.velocity.y < 2.0f && rb.velocity.y > -2.0f) {
				float newY = rb.velocity.y * 1.5f;
				rb.velocity = new Vector3 (rb.velocity.x, newY, rb.velocity.z);
			}
		}

		public void UpdatePosition(){
			//do nothing since rigidbody
		}




	}

	public class PlayerPhysics : IPhysics
	{



		private float speed = 0.25f;
		private Vector3 velocity;
		private Transform playerTrans;


		public PlayerPhysics (Transform playerTrans){
			this.playerTrans = playerTrans;
		}

		public void UpdateSpeed(float speed){
			this.speed = speed;
		}

		public void UpdateVelocity(Vector3 velocity){
			this.velocity = velocity;		
		}

		public void UpdatePosition(){
			if (playerTrans.position.y < -5.5f) {
				playerTrans.position = new Vector3(playerTrans.position.x,-5.5f,playerTrans.position.z);
			} else if (playerTrans.position.y > 5.5f) {
				playerTrans.position = new Vector3(playerTrans.position.x,5.5f,playerTrans.position.z);
			} else {
				playerTrans.Translate (this.velocity * this.speed);
			}
		}


	}
		
}
