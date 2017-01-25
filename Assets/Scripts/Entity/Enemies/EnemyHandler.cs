using System;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame
{
	public class EnemyHandler :MonoBehaviour
	{
		//Enemy prefab
		public Enemy enemyPrefabRef;

		//Enemy List
		private List<Enemy> enemies = new List<Enemy>();
		public int numEnemies = 1  ;

		//Set enemy speed (difficulty)
		[Range(0.001f, 0.5f)]
		public float difficulty = 0.15f;


		//on initialization, create all of your enemies
		void Start(){

            for (int i = 0; i < numEnemies; i++)
            {
                Enemy newEnemy = (Enemy)Instantiate(enemyPrefabRef);
                newEnemy.changeDiffculty(difficulty);
                enemies.Add(newEnemy);
            }
		}

	}
}

