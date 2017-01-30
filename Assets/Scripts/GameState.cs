using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

namespace CapstoneGame
{
    [XmlRoot("GameState")]
    public class GameState
    {
        [XmlArray("Balls")]
        [XmlArrayItem("Ball")]
        public List<BallState> balls;

        [XmlElement("Player")]
        public SerializableVector3 player;

        [XmlArray("Enemies")]
        [XmlArrayItem("Enemy")]
        public List<SerializableVector3> enemies;

        public int playerScore;
        public int cpuScore;

        public GameState()
        {

        }

    }
}


