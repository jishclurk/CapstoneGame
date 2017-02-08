using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public class SerializedPlayer {

    [XmlElement("In Control")]
    public bool isInControl;

    public List<int> Abilities;

    public SerializedPlayer()
    {

    }

}
