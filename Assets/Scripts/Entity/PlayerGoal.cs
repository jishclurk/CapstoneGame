using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame
{
    public class PlayerGoal : MonoBehaviour, IGoal
    {

        public GameManager manager;
        public AudioSource goalSound = new AudioSource();

        public void ScoreGoal()
        {
            goalSound.Play();
            manager.ScoreEnemyGoal();
        }

    }
}
