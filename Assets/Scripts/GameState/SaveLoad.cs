using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public static class SaveLoad {

    //Saves game in saveSpot
    public static void Save(GameState game, string name)
    {
        var serializer = new XmlSerializer(typeof(GameState));
        FileStream file = File.Create(Application.dataPath + "/savedGame" + name + ".gd");
        serializer.Serialize(file, game);
        file.Close();
    }

    //Loads game in saveSpot
    public static GameState Load(string name)
    {
        GameState gameState = null;

        if (File.Exists(Application.dataPath + "/savedGame" + name + ".gd"))
        {
            var serializer = new XmlSerializer(typeof(GameState));
            FileStream file = File.Open(Application.dataPath + "/savedGame" + name + ".gd", FileMode.Open);
            gameState = (GameState)serializer.Deserialize(file);
            file.Close();
        }
        return gameState;
    }

}
