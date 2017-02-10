using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager manager;

    public TeamManager teamManager;
    public ObjectiveManager objManager;
    public int currentCheckpoint;
    public Canvas SaveAsScreen;

    public bool newGame = true;
    private string gameName;

    //creates singleton, possibly change if universe asset is used
    public void Awake()
    {
        if (manager == null)
        {
            DontDestroyOnLoad(gameObject);
            manager = this;
        }
        else if (manager != this)
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

    //Sets the name for the game to be saved as
    public void setName(string name)
    {
        gameName = name;
        newGame = false;
        SaveAsScreen.enabled = false;
    }

    //if the game has never been saved, lets user save as, otherwise saves game as gameNfame
    public void OpenSaveScreen()
    {
        Debug.Log("open save screen");
        if (newGame)
        {
            SaveAsScreen = Instantiate(SaveAsScreen) as Canvas;
            SaveAsScreen.enabled = true;
        }
        else
        {
            SaveGame();
        }
    }

    //closes save screen
    public void CloseSaveScreen()
    {
        SaveAsScreen.enabled = false;
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


    public void SetTeamManager(TeamManager tm)
    {
        teamManager = tm;
    }

    public void SetObjectiveManager(ObjectiveManager om)
    {
        objManager = om;
    }
}
