using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame
{
    public class Enemy : MonoBehaviour, IEntity
    {

        private float speed = 0.25f;
       // private Vector3 velocity;
        public Vector3 velocity { get; set; }

        private EnemyAI enemyAI;
        //// Use this for initialization
        void Start()
        {
            velocity = new Vector3(0, 0, 0);
            this.enemyAI = new EnemyAI();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            enemyAI.UpdateAI(this);
            if (transform.position.y < -5.5f)
            {
                transform.position = new Vector3(transform.position.x, -5.5f, transform.position.z);
            }
            else if (transform.position.y > 5.5f)
            {
                transform.position = new Vector3(transform.position.x, 5.5f, transform.position.z);
            }
            else
            {
                transform.Translate(velocity * this.speed);
            }
        }

        public void changeDiffculty(float speed)
        {
            this.speed = speed;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

    }


    public class EnemyAI : IArtificialIntelligence
    {

        public ICommand UpdateAI(IEntity enemy)
        {
            Vector3 ballLocation = FindClosestBall(enemy.GetPosition());

            ICommand command = new DoNothing();
            if (ballLocation.y > enemy.GetPosition().y)
            {
                command = new MoveUp(enemy);

            }
            else if (ballLocation.y < enemy.GetPosition().y)
            {
                command = new MoveDown(enemy);
            }

            return command;
        }
        private Vector3 FindClosestBall(Vector3 enemyPosition)
        {
            GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
            if(balls.Length == 0)
            {
                return enemyPosition;
            }
            float smallestDistance = Vector3.Magnitude(balls[0].transform.position - enemyPosition);
            int closestBall = 0;
            for(int i = 0; i< balls.Length; i++)
            {
                float potentialSmallestDistance = Vector3.Magnitude(balls[i].transform.position - enemyPosition);
                if (potentialSmallestDistance < smallestDistance)
                {
                    smallestDistance = potentialSmallestDistance;
                    closestBall = i;
                }

            }

            return balls[closestBall].transform.position;
        
        }

        //private List<IEntity> watching;
        //private Transform enemyTrans;
        //private ICommand moveUp, moveDown;
        //private PlayerPhysics enemyPhys;

        //public EnemyAI(Transform enemyTrans, IPhysics enemyPhysics)
        //{
        //    this.enemyTrans = enemyTrans;
        //    this.enemyPhys = (PlayerPhysics)enemyPhysics;
        //    watching = new List<IEntity>();

        //}

        //public void AddEntityToWatch(IEntity watchingObj)
        //{
        //    //B
        //    Ball ball = (Ball)watchingObj;
        //    if (ball.ballObj != null)
        //    {
        //        if (watchingObj.GetTransform().position.x < 0)
        //        {

        //            ball.SetWatched(true);
        //            watching.Add(watchingObj);
        //            Debug.Log("Added a ball to enemy atch list");
        //        }
        //    }
        //}

        //public void ReleaseEntityToWatch(IEntity watchingObj)
        //{
        //    watching.Remove(watchingObj);
        //}

        //public List<IEntity> watchList()
        //{
        //    return watching;
        //}

        //public ICommand UpdateAI(IEntity enemy)
        //{

        //    List<IEntity> deleteList = new List<IEntity>();

        //    foreach (IEntity entity in watching)
        //    {

        //        if (entity is Ball)
        //        {
        //            Ball ball = (Ball)entity;
        //            //					if (ball.GetVelocity ().x < 0) {
        //            //						
        //            //					}
        //            if (ball.ballObj != null)
        //            {
        //                //General AI
        //                //						if (ball.GetVelocity ().x < 0) {
        //                if (ball.ballTrans.position.y > enemyTrans.position.y)
        //                {
        //                    return new MoveUp(enemy);
        //                    //enemyPhys.UpdateVelocity(new Vector3(0,1.0f,0));

        //                }
        //                else if (ball.ballTrans.position.y < enemyTrans.position.y)
        //                {
        //                    return new MoveDown(enemy);
        //                    //enemyPhys.UpdateVelocity(new Vector3(0,-1.0f,0));
        //                }
        //                //						} else {
        //                //							deleteList.Add (ball);
        //                //						}
        //            }
        //            else
        //            {
        //                //ball.SetWatched (false);
        //                deleteList.Add(ball);
        //            }
        //            //enemyPhys.UpdatePosition ();
        //            //
        //        }

        //        //add other watchable objects
        //    }

        //    foreach (IEntity entity in deleteList)
        //    {
        //        ReleaseEntityToWatch(entity);
        //    }

        //    return new DoNothing();

        //}


    }


}



