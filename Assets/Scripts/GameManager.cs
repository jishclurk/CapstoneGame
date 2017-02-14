using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //singleton instance
    public static GameManager manager;

    public TeamManager teamManager;
    public ObjectiveManager objManager;
    
	public int currentCheckpoint;
	public int level;

    public Canvas SaveAsScreen;
    public Canvas SavedSuccessfully;

	private bool newGame;
    public string gameName;
    public GameState LastSavedState { get; set; }

	private string[] levels;

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

    //saves the current state of the game
    private void SaveGame()
    {
        Debug.Log("SAVING GAME");
        Debug.Log(gameName);

        GameState gameState = new GameState();

        //gameState.players = teamManager.currentState();
        //gameState.objectives = objManager.currentState();
        //gameState.checkPoint = currentCheckpoint;

        SaveLoad.Save(gameState, gameName);
        SavedSuccessfully = Instantiate(SavedSuccessfully) as Canvas;
        SavedSuccessfully.enabled = true;
    }

    //Sets the name for the game to be saved as
    public void setNameAndSave(string name)
    {
        gameName = name;
		SaveGame ();
    }

    //if the game has never been saved, lets user save as, otherwise saves game as gameNfame
    public void OpenSaveScreen()
    {
        //newGame = true;
        if (newGame)
        {
            SaveAsScreen = Instantiate(SaveAsScreen) as Canvas;
            SaveAsScreen.enabled = true;
			newGame = false;
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

    public void MainMenu()
    {
        SavedSuccessfully.enabled = false;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
    }
  
    public void StartNewGame()
    {
        newGame = true;
        currentCheckpoint = 1;
		level = 1;
    }

    public void StartSavedGame()
    {
        
    } 

    public void LoadGameState(GameState state)
    {
        //in the future you can load the right scene for the checkpoint in state 

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