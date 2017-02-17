using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public static class SaveLoad
{

    //static SavedState lastSaved;

    //Saves game in saveSpot
    public static void Save(SavedState game, string name)
    {
        var serializer = new XmlSerializer(typeof(SavedState));
        FileStream file = File.Create(Application.dataPath + "/savedGame" + name + ".gd");
        serializer.Serialize(file, game);
        file.Close();
    }

    //Loads game in saveSpot
    public static SavedState Load(string name)
    {
        SavedState gameState = null;

        if (File.Exists(Application.dataPath + "/savedGame" + name + ".gd"))
        {
            var serializer = new XmlSerializer(typeof(SavedState));
            FileStream file = File.Open(Application.dataPath + "/savedGame" + name + ".gd", FileMode.Open);
            gameState = (SavedState)serializer.Deserialize(file);
            file.Close();
        }
        return gameState;
    }

    public static List<string> savedGames()
    {
        string[] nameArray = Directory.GetFiles(Application.dataPath + "/savedGame*");
        List<string> names = new List<string>(nameArray);
        for (int i = 0; i < names.Count; i++)
        {
            names[i] = names[i].Substring(names[i].IndexOf("/savedGame") + 10);
        }

        return names;
    }

}
