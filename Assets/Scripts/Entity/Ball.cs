using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame
{
    public class Ball : MonoBehaviour, IEntity {

        public AudioSource paddleHitAudio;
        public AudioSource wallHitAudio;

        public BallHandler spawner;

        public Vector3 velocity { get; set; }
        private Rigidbody rb; 
        private float speed=0.1f;

        // Use this for initialization
        void Start()
        {
            Debug.Log("Ball start");

            float i = Mathf.Pow(-1.0f, Random.Range(0, 2));
            //Debug.Log("x velocity" + i);
            float i2 = Mathf.Pow(-1.0f, Random.Range(0, 2));
            rb = this.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(i * Random.Range(12, 15), i2 * Random.Range(12, 15), 0); //random velocity

            paddleHitAudio = gameObject.GetComponents<AudioSource>()[0];
            wallHitAudio = gameObject.GetComponents<AudioSource>()[1];
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        //Moves Ball
        void FixedUpdate()
        {
            if (rb.velocity.x < 2f && rb.velocity.x > -2.0f)
            {
                float newX = rb.velocity.x * 1.5f;
                rb.velocity = new Vector3(newX, rb.velocity.y, 0);
            }

            if (rb.velocity.y < 2.0f && rb.velocity.y > -2.0f)
            {
                float newY = rb.velocity.y * 1.5f;
                rb.velocity = new Vector3(rb.velocity.x, newY, 0);
            }
        }

        //Handles collisions
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Goal") && spawner != null)
            {
                collision.gameObject.GetComponent<IGoal>().ScoreGoal();
                spawner.DestroyBall(this);
            }
            else if (collision.gameObject.CompareTag("Paddle"))
            {
                paddleHitAudio.Play();
            }
            else if (collision.gameObject.CompareTag("Wall"))
            {
                wallHitAudio.Play();
            }
        }
    }
}





