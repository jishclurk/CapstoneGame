using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    private float percentComplete;
    public Canvas Menu;  //main menu of game
    public Canvas Loading;  //canvus shows when a game is loading
    public Canvas LoadMenu;  //load a saved game
    private Text LoadingProgress;
    private Image LoadingBar;
    SimpleGameManager gm;
    private int levelToLoad;
    public Canvas Settings;

    public void Start()
    {
        //gm = GameObject.Find("GameManager").GetComponent<SimpleGameManager>();

        LoadMenu = LoadMenu.GetComponent<Canvas>();
        Menu = Menu.GetComponent<Canvas>();
        Loading = Loading.GetComponent<Canvas>();
        LoadingProgress = Loading.GetComponentInChildren<Text>();
        LoadingBar = Loading.GetComponentInChildren<Image>();
        percentComplete = 0;
        Settings = Settings.GetComponent<Canvas>();
        Settings.enabled = false;
        Loading.enabled = false;
        LoadMenu.enabled = false;
    }

    //Created a new SavedState and saves it under autosave.  Loads level 1 
    public void LoadNewGame()
    {
        SavedState newGame = new SavedState();
        newGame.name = "autosave";
        newGame.level = 1;
        newGame.checkPoint = 0;

        SerializedPlayer[] players = new SerializedPlayer[4];
        for(int i = 0; i<4; i++)
        {
            SerializedPlayer playerStart = new SerializedPlayer();
            playerStart.level = 1;
            playerStart.experience = 0;
            playerStart.statPoints = 5;
            playerStart.stamina = 1;
            playerStart.strength = 1;
            playerStart.intelligence = 1; 
            playerStart.id = i + 1;
            players[i] = playerStart;
            if(i == 0)
            {
                playerStart.isInControl = true;
            }
        }

        newGame.players = players;

        bool[] objectives = new bool[6] { false, false, false, false, false, false };
        newGame.objectives = objectives;

        Debug.Log("setting state change to Load level 1");
        levelToLoad = 1;
        SaveLoad.Save(newGame, "autosave");
        //  gm.newGame = true;
        // gm.OnStateChange += LoadLevel;
        //Play();
        LoadLevel();
    }


    public void LoadLevel()
    {
        Menu.enabled = false;
        Loading.enabled = true;

        StartCoroutine(LoadGame("Level" + levelToLoad.ToString()));
    }

    //Loads game 
    IEnumerator LoadGame(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while (!asyncLoad.isDone)
        {
            percentComplete = asyncLoad.progress;
            yield return null;
        }

    }

    //Brings up Load Menu
    public void StartLoadMenu()
    {
        Menu.enabled = false;
        LoadMenu.enabled = true;
        List<string> gameNames = SaveLoad.savedGames();
        Debug.Log(gameNames[0]);
    }

    public void CloseLoadMenu()
    {
        LoadMenu.enabled = false;
        Menu.enabled = true;
    }

    public void LoadSavedGame(string name)
    {
        LoadMenu.enabled = false;
        SavedState gameToLoad = SaveLoad.Load(name);
        Debug.Log(gameToLoad);
        SaveLoad.Save(gameToLoad, "autosave");
        levelToLoad = gameToLoad.level;
        LoadLevel();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    //Displays the percent loaded while game is loading
    void OnGUI()
    {
        LoadingProgress.text = "Loading... ";
        LoadingBar.fillAmount = percentComplete;
    }

    public void LoadSettingsMenu()
    {
        Debug.Log("loading settings menu");
        Menu.enabled = false;
        Settings.enabled = true;
    }

}
