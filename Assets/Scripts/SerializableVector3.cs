using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class SerializableVector3 {

    [XmlElement("x")]
    public float x;

    [XmlElement("y")]
    public float y;

    [XmlElement("z")]
    public float z;

    public SerializableVector3()
    {

    }

    public void setVector(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public Vector3 Deserialize()
    {
        return new Vector3(x, y, z);
    }
}
