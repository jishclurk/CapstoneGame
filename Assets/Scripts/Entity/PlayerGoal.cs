using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame
{
    public class PlayerGoal : MonoBehaviour, IGoal
    {

        public GameManager manager;

        public void ScoreGoal()
        {
            manager.ScorePlayerGoal();
        }

    }
}
