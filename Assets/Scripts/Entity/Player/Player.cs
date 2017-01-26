﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame
{
	public class Player : MonoBehaviour, IEntity
    {
		//private GameObject playerObj;
        public Vector3 velocity { get; set; }
        private float speed;

        public void Start()
        {
            speed = .5f;
            velocity = new Vector3(0, 0, 0);
        }

        public void FixedUpdate()
        {
            velocity *= speed;

            if (transform.position.y + velocity.y < -5.5f)
            {
                transform.position = new Vector3(transform.position.x, -5.5f, transform.position.z);
            }
            else if (transform.position.y + velocity.y > 5.5f)
            {
                transform.position = new Vector3(transform.position.x, 5.5f, transform.position.z);
            }
            else
            {
                transform.Translate(velocity);
            }
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
    }


}
