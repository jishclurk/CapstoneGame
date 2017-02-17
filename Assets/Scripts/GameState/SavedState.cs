﻿using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("GameState")]
public class SavedState
{

    [XmlElement("name")]
    public string name;

    [XmlElement("Level")]
    public int level;

    [XmlElement("Check Point")]
    public int checkPoint;

    [XmlArray("Players")]
    [XmlArrayItem("Player")]
    public SerializedPlayer[] players;

    [XmlArray("Objectives")]
    [XmlArrayItem("objectives")]
    public bool[] objectives;

    public SavedState()
    {

    }

    public void setFromGameManager()
    {
        SimpleGameManager gm = SimpleGameManager.Instance;
        this.name = gm.name;
        this.level = gm.level;
        this.checkPoint = gm.checkpoint;
    }

}
