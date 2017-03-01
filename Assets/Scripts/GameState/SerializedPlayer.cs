using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public class SerializedPlayer {

    [XmlElement("In Control")]
    public bool isInControl;

    public int level;
    public int experience;
    public int statPoints;
    public int strength;
    public int intelligence;
    public int stamina;
    public int id;

    public SerializedPlayer()
    {

    }


}
