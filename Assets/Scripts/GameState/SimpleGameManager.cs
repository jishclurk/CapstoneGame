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

    private float percentComplete;

    private TeamManager tm;

    private ObjectiveManager objManager;

    private void Start()
    {
        Debug.Log("start");
        tm = GameObject.Find("TeamManager").GetComponent<TeamManager>();
        cpManager = GameObject.Find("CheckpointManager").GetComponent<CheckpointManager>();
        SetSavedState(SaveLoad.Load("autosave"));
        objManager = GameObject.Find("ObjectiveManager").GetComponent<ObjectiveManager>();
    }

    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("level was loaded");
        //tm = GameObject.Find("TeamManager").GetComponent<TeamManager>();
        //cpManager = GameObject.Find("CheckpointManager").GetComponent<CheckpointManager>();
        //SetSavedState(SaveLoad.Load("autosave"));
        //objManager = GameObject.Find("ObjectiveManager").GetComponent<ObjectiveManager>();
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
        StartCoroutine(LoadScene("Level" + level.ToString()));
    }


    IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while (!asyncLoad.isDone)
        {
            percentComplete = asyncLoad.progress;
            yield return null;
        }

    }

    //after the level of saved in loaded, sets the state of the game
    public void SetSavedState(SavedState saved)
    {
        level = saved.level;
        tm.LoadSavedState(saved.players);
        cpManager.setState(saved.checkPoint);
        GameObject.Find("ObjectiveManager").GetComponent<ObjectiveManager>().loadState(saved.objectives);

    }

    public void onDeath()
    {
        Debug.Log("DEAD");
        SetSavedState(autosave);
    }

}