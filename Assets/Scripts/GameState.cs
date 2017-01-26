using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame
{
    [System.Serializable]
    public class GameState
    {
        public List<BallState> balls;
        public Vector3 player;
        public List<Vector3> enemies;
        public int playerScore;
        public int cpuScore;

        public GameState(List<BallState> balls, Vector3 player, List<Vector3> enemies, int playerScore, int cpuScore)
        {
            this.balls = balls;
            this.player = player;
            this.enemies = enemies;
            this.playerScore = playerScore;
            this.cpuScore = cpuScore;
        }

    }
}


