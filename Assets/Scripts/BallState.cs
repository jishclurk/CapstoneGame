using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame
{
    [System.Serializable]
    public class BallState
    {
        public Vector3 velocity;
        public Vector3 position;

        public BallState(Vector3 velocity, Vector3 position)
        {
            this.velocity = velocity;
            this.position = position;
        }

    }

}


