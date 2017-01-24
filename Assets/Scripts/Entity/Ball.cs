﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame
{
    public class Ball : MonoBehaviour, IEntity {

        AudioSource[] audio;

        public bool hitEnemy = false;
        public bool hitPlayer = false;
        public BallHandler spawner;

        public Vector3 velocity { get; set; }
        private Rigidbody rb; 
        private float speed=0.1f;

        // Use this for initialization
        void Start()
        {
            audio = gameObject.GetComponents<AudioSource>();
            float i = Mathf.Pow(-1.0f, Random.Range(0, 2));
            Debug.Log("x velocity" + i);
            float i2 = Mathf.Pow(-1.0f, Random.Range(0, 2));
            rb = this.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(i * Random.Range(8, 10), i2 * Random.Range(8, 10), 0); //random velocity
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
                spawner.DestroyBall(gameObject);
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





