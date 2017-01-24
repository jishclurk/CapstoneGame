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
		public int EnemyNumber { get { return numEnemies; } set  {numEnemies = value;}}

		//Set enemy speed (difficulty)
		[Range(0.001f,1.0f)]
		public float difficulty = 0.01f;


		//on initialization, create all of your enemies
		void Start(){
			//initialize enemies
			for (int i = 0; i < numEnemies; i++) {

                Enemy newEnemy = (Enemy)Instantiate(enemyPrefabRef);

                newEnemy.changeDiffculty (difficulty);
				enemies.Add(newEnemy);
			}


		}

        //add and remove new enemies potentially
		void Update()
		{


		}
	}
}

