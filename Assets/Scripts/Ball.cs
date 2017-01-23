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


}

