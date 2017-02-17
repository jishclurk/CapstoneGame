using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public enum GameState { INTRO, MAIN_MENU, PLAY, PAUSE, COMABT_PAUSE }

public delegate void OnStateChangeHandler();

//handles game state transitions and saving the game
public class SimpleGameManager {
    private Dictionary<int, int> checkPointsPerLevel = new Dictionary<int, int>() { { 1, 1 } };

	protected SimpleGameManager() {}

	//singleton 
	private static SimpleGameManager instance = null;

	//function called on state change
	public event OnStateChangeHandler OnStateChange;
	
    //current game state 
	public  GameState gameState { get; private set; }
	
    //false if it's new game that hasn't been saved yet
	public bool hasBeenSaved {get;set;}
	
    //name game is saved under
	public string name { get; set; }

	public int level { get; private set; }
	public int checkpoint{ get; set; } //current checkpoint that a player is working towards

	//last saved State
	private SavedState lastSavedState;

	public static SimpleGameManager Instance{
		get {
			if (SimpleGameManager.instance == null){
				//DontDestroyOnLoad(SimpleGameManager.instance);
				SimpleGameManager.instance = new SimpleGameManager();
			}
			return SimpleGameManager.instance;
		}

	}

    //State is changed and function set to OnStateChange is called
	public void SetGameState(GameState state){
        Debug.Log("changing state");
		this.gameState = state;
		OnStateChange();
        foreach(var function in OnStateChange.GetInvocationList())
        {
            OnStateChange -= (function as OnStateChangeHandler);
        }
	}

	public void OnApplicationQuit(){
		SimpleGameManager.instance = null;
	}

	public void NextCheckPointOrLevel(){
        Debug.Log("Next checkpoint or level");
        Time.timeScale = 1;
        if(checkPointsPerLevel[level] == checkpoint)
        {
            level++;
            checkpoint = 0;
            LoadLevel(level);
        }
        else
        {
            checkpoint ++;
        }
      
	}

	public static void LoadLevel(int level){
		SceneManager.LoadScene ("Level" + level.ToString());
	}

	public void LoadSavedGame(SavedState saved){
		name = saved.name;
		level = saved.level;
		checkpoint = saved.checkPoint;
		lastSavedState = saved;
		LoadLevel (level);
		//set team manager
		//move to checkpoint
		//set objectives

	}

	public void NewGame(){
		name = null;
		hasBeenSaved = false;
		checkpoint = 1;
		level = 1;
	}

}