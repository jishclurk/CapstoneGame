using System;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame
{
	public class EnemyHandler :MonoBehaviour
	{
		public GameObject enemyPrefab;
		public List<Enemy> enemies;
		[Range(1,3)]
		public int numEnemies = 1  ;
		[Range(0.001f,1.0f)]
		public float difficulty = 0.01f;
		public BallHandler ballHandler;


		void Start(){
			enemies = new List<Enemy> ();
			for (int i = 0; i < numEnemies; i++) {

				Enemy newEnemy = new Enemy (Instantiate (enemyPrefab, new Vector3 (-15.0f, 0+2*i, 0), transform.rotation));
				Ball unwatchedBall = ballHandler.GetUnWatchedBall ();
				if (unwatchedBall != null){
					newEnemy.enemyAI.AddEntityToWatch (unwatchedBall);
				}
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
				enemy.UpdateAI ().Execute(enemy);
			}

		}
	}
}

