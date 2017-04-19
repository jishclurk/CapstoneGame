using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public static class SaveLoad
{

    //static SavedState lastSaved;

    //Saves game in saveSpot
    public static void Save(SavedState game, int spot)
    {
        var serializer = new XmlSerializer(typeof(SavedState));
        FileStream file = File.Create(Application.dataPath + "/savedGame" + spot.ToString()  + ".gd");
        serializer.Serialize(file, game);
        file.Close();
    }

    //Loads game in saveSpot
    public static SavedState Load(int slot)
    {
        SavedState gameState = null;

        if (File.Exists(Application.dataPath + "/savedGame" + slot.ToString() + ".gd"))
        {
            var serializer = new XmlSerializer(typeof(SavedState));
            FileStream file = File.Open(Application.dataPath + "/savedGame" + slot.ToString() + ".gd", FileMode.Open);
            gameState = (SavedState)serializer.Deserialize(file);
            file.Close();
        }
        return gameState;
    }

    public static bool setSaveButton(SaveButton toSet)
    {
        bool savedInSlot = true;
        if (File.Exists(Application.dataPath + "/savedGame" + toSet.slotNumber.ToString() + ".gd"))
        {
            Debug.Log("1111");
            SavedState state = Load(toSet.slotNumber);
            toSet.Title.enabled = false;
            toSet.Date.enabled = true;
            toSet.Level.enabled = true;
            toSet.Date.text = state.date;
            toSet.Level.text = "Level " + state.level.ToString();
        }else
        {
            Debug.Log("22222");
            savedInSlot = false;

            toSet.Title.enabled = true;
            toSet.Date.enabled = false;
            toSet.Level.enabled = false;

            //nothing saved in slot
        }
        return savedInSlot;

    }

    public static List<string> savedGames()
    {
        string[] nameArray = Directory.GetFiles(Application.dataPath, "savedGame*.gd");
        List<string> names = new List<string>(nameArray);
//        for (int i = 0; i < names.Count; i++)
//        {
//          //  names[i] = names[i].Substring(names[i].IndexOf("/savedGame") + 10);
//			names[i] = names[i].Substring(names[i].IndexOf("") + 10);
//        }

        return names;
    }

}
