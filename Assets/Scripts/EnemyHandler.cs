using System;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame
{
	public class EnemyHandler :MonoBehaviour
	{
		//Enemy prefab
		public GameObject enemyPrefab;
		//Enemy List
		private List<Enemy> enemies;


		private int numEnemies = 1  ;

		public int EnemyNumber { get { return numEnemies; } set  {numEnemies = value;}}

		//Set enemy speed (difficulty)
		[Range(0.001f,1.0f)]
		public float difficulty = 0.01f;

		//this is the object that keeps track of all of the balls in play
		public BallHandler ballHandler;

		//on initialization, create all of your enemies
		void Start(){
			//initialize enemies
			enemies = new List<Enemy> ();
			for (int i = 0; i < numEnemies; i++) {

				Enemy newEnemy = new Enemy (Instantiate (enemyPrefab, new Vector3 (-15.0f, 0+2*i, 0), transform.rotation));

				//Using the ball handler, find a ball that is not being tracked by an enemy, and link the enemy to that ball

//				Ball unwatchedBall = ballHandler.GetUnWatchedBall ();
//				if (unwatchedBall != null){
//					newEnemy.enemyAI.AddEntityToWatch (unwatchedBall);
//				}

				//
				newEnemy.changeDiffculty (difficulty);
				enemies.Add(newEnemy);
			}


		}

		void FixedUpdate()
		{

			HandleEnemies ();

		}

		public void HandleEnemies(){
			foreach (Enemy enemy in enemies){
				if (enemy.enemyAI.watchList ().Count == 0) {

					Ball unwatchedBall = ballHandler.GetUnWatchedBall ();
					if (unwatchedBall != null){
						enemy.enemyAI.AddEntityToWatch (unwatchedBall);


					}
				}
				enemy.UpdateAI ().Execute(enemy);

				//enemy.UpdateAI ();
			}

		}
	}
}

