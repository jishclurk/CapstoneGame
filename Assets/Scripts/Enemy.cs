using System;
using System.Collections.Generic;
using UnityEngine;


namespace CapstoneGame{
	
	public class Enemy : IEntity
	{
		private GameObject enemyObj;

		public Transform enemyTrans;

		public IPhysics enemyPhys;




		private bool watched;

		public IArtificialIntelligence enemyAI;
		
		public Enemy (GameObject enemyObj)
		{
			this.enemyObj = enemyObj;
			this.enemyTrans = enemyObj.transform;
			this.enemyPhys = new PlayerPhysics (this.enemyTrans);
			this.enemyAI = new EnemyAI (this.enemyTrans,this.enemyPhys);
			watched = false;
		}

		public void UpdatePhysics (Vector3 velocity){
			this.enemyPhys.UpdateVelocity (velocity);
			this.enemyPhys.UpdatePosition ();
		}

		public Command UpdateAI(){
			//nothing here - for co-opping AI
			//enemyAI
			return enemyAI.UpdateAI();
		}

		public bool BeingWatched(){
			return this.watched;
		}

		public void SetWatched(bool watched){
			this.watched = watched;
		}

		public Transform GetTransform(){
			return this.enemyTrans;
		}

		public void changeDiffculty(float speed){
			this.enemyPhys.UpdateSpeed (speed);
		}
	}

	public class EnemyAI : IArtificialIntelligence
	{
		private List<IEntity> watching;
		private Transform enemyTrans;
		private Command moveUp, moveDown;
		private PlayerPhysics enemyPhys;

		public EnemyAI(Transform enemyTrans, IPhysics enemyPhysics){
			this.enemyTrans = enemyTrans;
			this.enemyPhys = (PlayerPhysics)enemyPhysics;
			watching = new List<IEntity> ();

		}

		public void AddEntityToWatch(IEntity watchingObj){
			//B
			Ball ball = (Ball)watchingObj;
			if (ball.ballObj != null) {
				if (watchingObj.GetTransform ().position.x < 0) {
				
					ball.SetWatched (true);
					watching.Add (watchingObj);
					Debug.Log ("Added a ball to enemy atch list");
				}
			}
		}

		public void ReleaseEntityToWatch(IEntity watchingObj){
			watching.Remove (watchingObj);
		}

		public List<IEntity> watchList(){
			return watching;
		}

		public Command UpdateAI(){

			List<IEntity> deleteList = new List<IEntity> ();

			foreach (IEntity entity in watching) {

				if (entity is Ball) {
						Ball ball = (Ball)entity;
//					if (ball.GetVelocity ().x < 0) {
//						
//					}
					if (ball.ballObj != null) {
						//General AI
//						if (ball.GetVelocity ().x < 0) {
							if (ball.ballTrans.position.y > enemyTrans.position.y) {
								return new MoveUp ();
								//enemyPhys.UpdateVelocity(new Vector3(0,1.0f,0));
							
							} else if (ball.ballTrans.position.y < enemyTrans.position.y) {
								return new MoveDown ();
								//enemyPhys.UpdateVelocity(new Vector3(0,-1.0f,0));
							}
//						} else {
//							deleteList.Add (ball);
//						}
					} else {
						//ball.SetWatched (false);
						deleteList.Add (ball);
					}
					//enemyPhys.UpdatePosition ();
					//
				}

				//add other watchable objects
			}

			foreach (IEntity entity in deleteList) {
				ReleaseEntityToWatch (entity);
			}

			return new DoNothing ();

		}


	}
}

