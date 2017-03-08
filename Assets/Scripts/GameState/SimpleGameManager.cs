using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public enum GameState { INTRO, MAIN_MENU, PLAY, PAUSE, COMABT_PAUSE }

public delegate void OnStateChangeHandler();

//handles game state transitions and saving the game
public class SimpleGameManager : MonoBehaviour
{
    private Dictionary<int, int> checkPointsPerLevel = new Dictionary<int, int>() { { 1, 1 } };

    protected SimpleGameManager() { }

    //singleton 
    private static SimpleGameManager instance = null;

    //function called on state change
    public event OnStateChangeHandler OnStateChange;

    //current game state 
    public GameState gameState { get; private set; }

    //false if it's new game that hasn't been saved yet
    public bool hasBeenSaved { get; set; }

    public int level;

    public bool newGame;

    private CheckpointManager cpManager;

    //last saved State
    public SavedState lastSavedState;

    //autosaved state from last cp 
    private SavedState autosave;


    void Awake()
    {
        newGame = false;
        DontDestroyOnLoad(this);
        Debug.Log("HERE 111111111111");
        level = 0;
        //checkpoint = 0;
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }

    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("on level was loaded 888888888");
        if (gameState != GameState.MAIN_MENU)
        {
            cpManager = GameObject.Find("CheckpointManager").GetComponent<CheckpointManager>();
            if (newGame)
            {
                Debug.Log("gm is loading new game");
                NewGame();
            }
            else
            {
                newGame = false;
                Debug.Log("gm is loading saved state");
                SetSavedState(lastSavedState);

            }

        }

    }

    private void Start()
    {
        Debug.Log("HERE 0000000000000000000000");
    }

    public static SimpleGameManager Instance
    {
        get
        {
            if (SimpleGameManager.instance == null)
            {
                //DontDestroyOnLoad(SimpleGameManager.instance);
                Debug.Log("gm instance is null");
                SimpleGameManager.instance = new SimpleGameManager();
            }
            return SimpleGameManager.instance;
        }

    }


    //State is changed and function set to OnStateChange is called
    public void SetGameState(GameState state)
    {
        Debug.Log("changing state");

        this.gameState = state;
        OnStateChange();
        foreach (var function in OnStateChange.GetInvocationList())
        {
            OnStateChange -= (function as OnStateChangeHandler);
        }
    }

    //shuts down game
    public void OnApplicationQuit()
    {
        SimpleGameManager.instance = null;
    }

    public void nextLevel()
    {
        Debug.Log("next level");

        //save with level ++
        //load level
    }
    //Loads Level level
    public static void LoadLevel(int level)
    {
        SceneManager.LoadScene("Level" + level.ToString());
    }

    //after the level of saved in loaded, sets the state of the game
    public void SetSavedState(SavedState saved)
    {
       // names = saved.name;
        level = saved.level;
        lastSavedState = saved;
        GameObject.Find("TeamManager").GetComponent<TeamManager>().LoadSavedState(saved.players);
        cpManager.setState(saved.checkPoint);
        GameObject.Find("ObjectiveManager").GetComponent<ObjectiveManager>().loadState(saved.objectives);
        //move to checkpoint
        //set objectives

    }

    //sets up new game
    public void NewGame()
    {
        //create last saved start as start state 
        Debug.Log("new game");
        //name = null;
        hasBeenSaved = false;
        level = 1;
        lastSavedState = new SavedState();
        lastSavedState.level = level;
        lastSavedState.checkPoint = 0;
        lastSavedState.players = GameObject.Find("TeamManager").GetComponent<TeamManager>().currentState();
    }

    public void onDeath()
    {
        //ask for state you wanna load, save slots or auto save
        //set lastSavedState as the savedState selected
        //loadLevel
    }

    public void AutoSave()
    {
        //  TO DO
    }

    public int getCheckpoint()
    {
        return cpManager.completed;
    }

}