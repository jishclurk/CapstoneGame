using System;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame
{
	public class Player : IEntity
	{
		private GameObject playerObj;
		private bool watched;
		public Transform playerTrans;

		public IPhysics playerPhys;



		public Player (GameObject playerObj)
		{
			
			this.playerObj = playerObj;
			this.playerTrans = playerObj.transform;
			this.playerPhys = new PlayerPhysics (this.playerTrans);
			watched = false;
			
		}

		public void UpdatePhysics(Vector3 velocity){
			playerPhys.UpdateVelocity (velocity);
			playerPhys.UpdatePosition ();
		}

		public Command UpdateAI(){
			//nothing here - for co-opping AI
			return new DoNothing();
		}

		public bool BeingWatched(){
			return this.watched;
		}

		public void SetWatched(bool watched){
			this.watched = watched;
		}

		public Transform GetTransform(){
			return this.playerTrans;
		}

		public void changeSpeed(float speed){
			playerPhys.UpdateSpeed (speed);
		}


	}


}

