using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour{

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

	public void LoadNewGame(){
		gm.OnStateChange += LoadLevel1;
		Play ();
	}


	public void LoadLevel1(){
		Menu.enabled = false;
		Loading.enabled = true;
		StartCoroutine(LoadGame("Level1"));
		//GameManager.manager.StartNewGame();
	}

	private void Play(){
		gm.SetGameState(State.PLAY);
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
    }

    public void LoadSavedGame(string name)
    {
		
        List<string> gameNames = SaveLoad.savedGames();
        if (gameNames.Contains(name))
        {
			//gm.OnStateChange += SaveLoad
           // GameManager.manager.LastSavedState = SaveLoad.Load(name);
          //  StartCoroutine(LoadGame("test"));
           //GameManager.manager.StartSavedGame();
        }

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
