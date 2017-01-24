using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame
{
    public class Ball : MonoBehaviour, IEntity {

        AudioSource[] audio;

        public bool hitEnemy = false;
        public bool hitPlayer = false;

        public Vector3 velocity { get; set; }
        private Rigidbody rb; 
        private float speed;

        // Use this for initialization
        void Start()
        {
            audio = gameObject.GetComponents<AudioSource>();
            float i = Mathf.Pow(-1.0f, Random.Range(0, 2));
            Debug.Log("x velocity" + i);
            float i2 = Mathf.Pow(-1.0f, Random.Range(0, 2));
            rb = GetComponent<Rigidbody>();
            rb.velocity = new Vector3(i * Random.Range(3, 6), i2 * Random.Range(3, 5), 0); //random velocity
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        //Moves Ball
        void FixedUpdate()
        {
            rb.velocity = rb.velocity * speed;

            if (rb.velocity.x < 2f && rb.velocity.x > -2.0f)
            {
                float newX = rb.velocity.x * 1.5f;
                rb.velocity = new Vector3(newX, rb.velocity.y, rb.velocity.z);
            }

            if (rb.velocity.y < 2.0f && rb.velocity.y > -2.0f)
            {
                float newY = rb.velocity.y * 1.5f;
                rb.velocity = new Vector3(rb.velocity.x, newY, rb.velocity.z);
            }
        }

        //Handles collisions
        void OnCollisionEnter(Collision collision)
        {
            string name = collision.gameObject.name;
            if (name.Contains("Enemy_Goal"))
            {
                hitEnemy = true;

            }
            else if (name.Contains("Player_Goal"))
            {
                hitPlayer = false;

            }
            else if (name.Contains("Paddle"))
            {
                audio[0].Play();
            }
            else if (name.Contains("Wall"))
            {
                audio[1].Play();
            }
        }
    }
}


    //public void UpdateSpeed(float speed)
    //{
    //    this.speed = speed;
    //}

    //public void UpdateVelocity(Vector3 velocity)
    //{
    //    rb.velocity = rb.velocity * speed;

    //    if (rb.velocity.x < 2f && rb.velocity.x > -2.0f)
    //    {
    //        float newX = rb.velocity.x * 1.5f;
    //        rb.velocity = new Vector3(newX, rb.velocity.y, rb.velocity.z);
    //    }

    //    if (rb.velocity.y < 2.0f && rb.velocity.y > -2.0f)
    //    {
    //        float newY = rb.velocity.y * 1.5f;
    //        rb.velocity = new Vector3(rb.velocity.x, newY, rb.velocity.z);
    //    }
    //}




