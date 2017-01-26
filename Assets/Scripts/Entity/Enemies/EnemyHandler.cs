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

        public List<SerializableVector3> saveState()
        {
            List<SerializableVector3> savedState = new List<SerializableVector3>();
            foreach(Enemy enemy in enemies)
            {
                SerializableVector3 position = new SerializableVector3();
                position.setVector(enemy.GetPosition());
                savedState.Add(position);
            }

            return savedState;
        }

        public void setState(List<SerializableVector3> positions)
        {
            if(enemies.Count != 0)
            {
                for(int i = 0; i<enemies.Count; i++)
                {
                    Destroy(enemies[i].gameObject);
                    enemies.RemoveAt(i);
                    i--;
                }

            }

            foreach (SerializableVector3 position in positions)
            {
                Enemy newEnemy = (Enemy)Instantiate(enemyPrefabRef);
                newEnemy.changeDiffculty(difficulty);
                newEnemy.SetPosition(position.Deserialize());
                enemies.Add(newEnemy);
            }
        }

        public void Restart()
        {

        }

    }
}

