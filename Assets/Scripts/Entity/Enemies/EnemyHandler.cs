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

        public List<Vector3> saveState()
        {
            List<Vector3> savedState = new List<Vector3>();
            foreach(Enemy enemy in enemies)
            {
                savedState.Add(enemy.GetPosition());
            }

            return savedState;
        }

        public void setState(List<Vector3> positions)
        {
            if(enemies.Count != 0)
            {
                foreach (Enemy enemy in enemies)
                {
                    enemies.Remove(enemy);
                    Destroy(enemy);
                }

            }

            foreach (Vector3 position in positions)
            {
                Enemy newEnemy = (Enemy)Instantiate(enemyPrefabRef);
                newEnemy.changeDiffculty(difficulty);
                newEnemy.SetPosition(position);
                enemies.Add(newEnemy);
            }
        }

    }
}

