using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("GameState")]
public class GameState {

    [XmlElement("Check Point")]
    public int checkPoint;

    [XmlArray("Players")]
    [XmlArrayItem("Player")]
    public SerializedPlayer[] players;

    [XmlArray("Objectives")]
    [XmlArrayItem("Objective")]
    public List<bool> objectives; 

    public GameState()
    {

    }
}
