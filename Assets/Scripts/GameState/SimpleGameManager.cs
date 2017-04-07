using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

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

    public Canvas GameOverScreen;

    public Canvas LoadingScreen;
    private Text LoadingProgress;
    private Image LoadingBar;

    private void Start()
    {

        Debug.Log("start");
        tm = GameObject.Find("TeamManager").GetComponent<TeamManager>();
        cpManager = GameObject.Find("CheckpointManager").GetComponent<CheckpointManager>();
        SetSavedState(SaveLoad.Load("autosave"));
        objManager = GameObject.Find("ObjectiveManager").GetComponent<ObjectiveManager>();
        GameOverScreen = transform.GetChild(0).GetComponent<Canvas>();
        GameOverScreen.enabled = false;
        LoadingScreen = LoadingScreen.GetComponent<Canvas>();
        LoadingProgress = LoadingScreen.GetComponentInChildren<Text>();
        LoadingBar = LoadingScreen.transform.GetChild(1).GetComponent<Image>();
        LoadingScreen.enabled = false;
        percentComplete = 0f;
    }

    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("level was loaded");
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
        Debug.Log("here");
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
        LoadingScreen.enabled = true;
        Debug.Log("Loading " + sceneName);
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
        Debug.Log( tm);

        Debug.Log("active player" + tm.activePlayer.gameObject);
        Camera.main.GetComponent<OffsetCamera>().setCamera(tm.activePlayer.gameObject);

    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void unPause()
    {
        Time.timeScale = 1;
    }

    public void onDeath()
    {
        OnStateChange += Pause;
        SetGameState(GameState.PAUSE);
        Debug.Log("DEAD");
        GameOverScreen.enabled = true;
    }

    public void ReloadLevel()
    {
        if(autosave == null)
        {
            autosave = SaveLoad.Load("autosave");
        }
        OnStateChange += unPause;
        SetGameState(GameState.PLAY);
        StartCoroutine(LoadScene("Level" + autosave.level));
    }

    public void toMain()
    {
        OnStateChange += unPause;
        SetGameState(GameState.MAIN_MENU);
        Debug.Log("here 2");
        StartCoroutine(LoadScene("MainMenu"));
    }

    public void LoadAutoSave()
    {

        //OnStateChange += unPause;
       // SetGameState(GameState.PLAY);
        GameOverScreen.enabled = false;
        SavedState autosave = SaveLoad.Load("autosave");
        SetSavedState(autosave);
    }

    void OnGUI()
    {
        LoadingProgress.text = "Loading... ";
        LoadingBar.fillAmount = percentComplete;
    }
}