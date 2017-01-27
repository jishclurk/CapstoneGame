using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;

namespace CapstoneGame
{
    public static class SaveLoad
    {

        //Saves game in saveSpot
        public static void Save(GameState game, int saveSpot)
        {
            var serializer = new XmlSerializer(typeof(GameState));
            FileStream file = File.Create(Application.dataPath + "/savedGame" + saveSpot + ".gd");
            serializer.Serialize(file, game);
            file.Close();
        }
        
        //Loads game in saveSpot
        public static GameState Load(int saveSpot)
        {
            GameState gameState = null;

            if (File.Exists(Application.dataPath + "/savedGame" + saveSpot + ".gd"))
            {
                var serializer = new XmlSerializer(typeof(GameState));
                FileStream file = File.Open(Application.persistentDataPath + "/savedGame" + saveSpot + ".gd", FileMode.Open);
                gameState = (GameState)serializer.Deserialize(file);
                file.Close();
            }
            return gameState;
        }

    }
}


