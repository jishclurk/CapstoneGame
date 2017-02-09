using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager manager;

    public TeamManager teamManager;
    public ObjectiveManager objManager;
    public int currentCheckpoint;
    public Canvas SaveAsScreen;

    public bool newGame;
    private string gameName;

    //creates singleton, possibly change if universe assert is used
    public void Awake()
    {
        SaveAsScreen = Instantiate(SaveAsScreen) as Canvas;
        SaveAsScreen = SaveAsScreen.GetComponent<Canvas>();
        SaveAsScreen.enabled = false;
        newGame = true;
        if (manager == null)
        {
            DontDestroyOnLoad(gameObject);
            manager = this;
        }
        else if(manager != this)
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
       
    }
    //saves the current state of the game
    private void SaveGame()
    {

        GameState gameState = new GameState();

        //get player states

        //get objective states

        //get checkpoint

        SaveLoad.Save(gameState, gameName);
    }

    public void setName (string name)
    {
        gameName = name;
        newGame = false;
        SaveAsScreen.enabled = false;
    }

    //if the game has never been saved, lets user save as, otherwise saves game 
    public void OpenSaveScreen()
    {
        if (newGame)
        {
            SaveAsScreen.enabled = true;
        }
        else{
            SaveGame();
        }
    }

    public void nextLevel()
    {

    }

    public void openSavedGame(string name)
    {
        SaveLoad.Load(name);
    }

    public void StartNewGame()
    {
        newGame = true;
        currentCheckpoint = 1;
    }

}
