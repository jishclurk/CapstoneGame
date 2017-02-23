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

    public void Start()
    {
        gm = SimpleGameManager.Instance;

        LoadMenu = LoadMenu.GetComponent<Canvas>();
        Menu = Menu.GetComponent<Canvas>();
        Loading = Loading.GetComponent<Canvas>();
        LoadingProgress = Loading.GetComponentInChildren<Text>();
        LoadingBar = Loading.GetComponentInChildren<Image>();
        percentComplete = 0;

        Loading.enabled = false;
        LoadMenu.enabled = false;
    }

    public void LoadNewGame()
    {
        Debug.Log("setting state change to Load level 1");
        gm.OnStateChange += LoadLevel1;
        Play();
    }


    public void LoadLevel1()
    {
        Debug.Log("loading level 1");
        Menu.enabled = false;
        Loading.enabled = true;
        gm.NewGame();
        StartCoroutine(LoadGame("Level1Final"));
        //GameManager.manager.StartNewGame();
    }

    private void Play()
    {
        gm.SetGameState(GameState.PLAY);
    }

    //Loads game 
    IEnumerator LoadGame(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while (!asyncLoad.isDone)
        {
            percentComplete = asyncLoad.progress;
            Debug.Log(percentComplete);
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
        //created buttons for each saved game

    }

    public void LoadSavedGame(string name)
    {

        List<string> gameNames = SaveLoad.savedGames();
        Debug.Log(gameNames);
        if (gameNames.Contains(name))
        {
            //gm.OnStateChange += gm.LoadSavedGame (SaveLoad.Load (name));

        }
        Play();

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

}
