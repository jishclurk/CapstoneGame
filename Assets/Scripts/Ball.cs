using System;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame
{
	public class Ball : IEntity
	{

		public Transform ballTrans;

		public GameObject ballObj;

		public IPhysics ballPhys;

		private Rigidbody rb;

		private bool watched;

		public Ball (GameObject ball)
		{
			this.ballObj = ball;

			this.rb = ballObj.GetComponent<Rigidbody> ();
			this.ballPhys = new BallPhysics (this.rb);
			ballTrans = this.ballObj.transform;
			watched = false;
		}

		public bool BeingWatched(){
			return this.watched;
		}

		public void SetWatched(bool watched){
			this.watched = watched;
		}

		public Transform GetTransform(){
			return this.ballTrans;
		}

		public Command UpdateAI(){
			//nothign here
			return new DoNothing();
		}

		public void UpdatePhysics(Vector3 velocity){
			//nothing here yet
			ballPhys.UpdateVelocity(rb.velocity);

		}

		public void SetSpeedUpConstant(float speed){
			ballPhys.UpdateSpeed (speed);
		}
		public Vector3 GetVelocity(){
			return rb.velocity;
		}
	}

	public class BallPhysics : IPhysics
	{



//		[Range(1.0010f,1.0100f)]
//		public float speedUp = 1.0010f;
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
}

