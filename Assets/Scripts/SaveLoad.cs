using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace CapstoneGame
{
    public static class SaveLoad
    {

        //Saves game in saveSpot
        public static void Save(GameState game, int saveSpot)
        {
            //savedGames.Add(game);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/savedGame" + saveSpot + ".gd");
            bf.Serialize(file, game);
            file.Close();
        }
        
        //Loads game in saveSpot
        public static GameState Load(int saveSpot)
        {
            GameState gameState = null;

            if (File.Exists(Application.persistentDataPath + "/savedGame" + saveSpot + ".gd"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/savedGame" + saveSpot + ".gd", FileMode.Open);
                gameState = (GameState)bf.Deserialize(file);
                file.Close();
            }
            return gameState;
        }

    }
}


