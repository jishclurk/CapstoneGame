using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

namespace CapstoneGame
{
    public class BallState
    {
        [XmlElement("velocity")]
        public SerializableVector3 velocity;

        [XmlElement("position")]
        public SerializableVector3 position;

        public BallState()
        {
        }

    }

}


