﻿using System.Collections;
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

    public void Start()
    {
        LoadMenu = LoadMenu.GetComponent<Canvas>();
        Menu = Menu.GetComponent<Canvas>();
        Loading = Loading.GetComponent<Canvas>();
        LoadingProgress = Loading.GetComponentInChildren<Text>();
        percentComplete = 0;

        Loading.enabled = false;
        LoadMenu.enabled = false;
    }

    //Loads test level (will be start new game in the future)
    public void LoadTestGame()
    {
        Menu.enabled = false;
        Loading.enabled = true;
        StartCoroutine(LoadGame("test"));
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

    //Displays the percent loaded while game is loading
    void OnGUI()
    {
        LoadingProgress.text = "Loading... " + percentComplete.ToString() + "%";
    }


}