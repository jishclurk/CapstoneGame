using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public enum GameState { INTRO, MAIN_MENU, PLAY, PAUSE, COMABT_PAUSE }

public delegate void OnStateChangeHandler();

//handles game state transitions and saving the game
public class SimpleGameManager : MonoBehaviour
{
    //function called on state change
    public event OnStateChangeHandler OnStateChange;

    //current game state 
    public GameState gameState { get; private set; }

    public int level;

    private CheckpointManager cpManager;

    //autosaved state from last cp 
    public SavedState autosave;


    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("level was loaded");
        cpManager = GameObject.Find("CheckpointManager").GetComponent<CheckpointManager>();
        SetSavedState(SaveLoad.Load("autosave"));

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

    public void nextLevel()
    {
        level++;
        Debug.Log("advance to level " + level);
        autosave.checkPoint = 0;
        autosave.level++;
     //   LoadLevel(level);
    }

    //Loads Level level
    public static void LoadLevel(int level)
    {
        SceneManager.LoadScene("Level" + level.ToString());
    }

    //after the level of saved in loaded, sets the state of the game
    public void SetSavedState(SavedState saved)
    {
        level = saved.level;
        GameObject.Find("TeamManager").GetComponent<TeamManager>().LoadSavedState(saved.players);
        cpManager.setState(saved.checkPoint);
        GameObject.Find("ObjectiveManager").GetComponent<ObjectiveManager>().loadState(saved.objectives);

    }

    //sets up new game
    //public void NewGame()
    //{
    //    //create last saved start as start state 
    //    Debug.Log("new game");
    //    //name = null;
    //    //hasBeenSaved = false;
    //    level = 1;
    //    autosave = new SavedState();
    //    autosave.level = level;
    //    autosave.checkPoint = 0;
    //    autosave.players = GameObject.Find("TeamManager").GetComponent<TeamManager>().currentState();
    //    autosave.objectives = GameObject.Find("ObjectiveManager").GetComponent<ObjectiveManager>().currentState();
    //}

    public void onDeath()
    {
        Debug.Log("DEAD");
        SetSavedState(autosave);
    }

    public void AutoSave()
    {
        //  TO DO
    }
}