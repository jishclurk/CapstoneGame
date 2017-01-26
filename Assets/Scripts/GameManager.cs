﻿using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace CapstoneGame {
	
    public class GameManager : MonoBehaviour {

	    private int enemyScore;
	    private int playerScore;
	    private int endScore = 5;
        private bool paused = true;

        public Scoreboard scoreboard;
        public GameObject MainMenu;
        public GameObject PauseMenu;
        public GameObject GameResultMenu;
        public GameObject SaveMenu;
        public GameObject LoadScreen;

        public BallHandler bh;
        public EnemyHandler eh;
        public PlayerHandler ph;

        public AudioSource select;
        public AudioSource hover;

        //Brings up Main Menu
        void Start () {
		    enemyScore = 0;
		    playerScore = 0;
            LoadMainMenu();
        }

        //Checks for game state changes (playing and pausing)
        void Update () {
            if (!MainMenu.activeSelf && Input.GetKeyDown(KeyCode.Space))
            {
                if (paused)
                {
                    ResumeGame();

                }else
                {
                    Pause();
                }
            }

	    }

        public void ScoreEnemyGoal()
        {
            enemyScore++;
            scoreboard.updateEnemyScore(enemyScore);
            if (enemyScore > 10) //End the game at 11 points.
            {
                EndGame();
            }
        }

        public void ScorePlayerGoal()
        {
            playerScore++;
            scoreboard.updatePlayerScore(playerScore);
            if(playerScore > 10)
            {
                EndGame();
            }
        }

        //Pauses game and brings up pause menu
        public void Pause()
        {
            SaveMenu.SetActive(false);
            Time.timeScale = 0;
            paused = true;
            PauseMenu.SetActive(true);
        }

        //Brings down pause menu and opens save menu
        public void LoadSaveMenu()
        {
            PauseMenu.SetActive(false);
            SaveMenu.SetActive(true);
        }

        //Saves game in saveSpot
        public void SaveGame(int saveSpot)
        {
            GameState currentState = new GameState();
            currentState.balls = bh.saveState();
            SerializableVector3 player = new SerializableVector3();
            player.setVector(ph.saveState());
            currentState.player = player;
            currentState.enemies = eh.saveState();
            currentState.playerScore = playerScore;
            currentState.cpuScore = enemyScore;
            SaveLoad.Save(currentState, saveSpot);
        }

        //Loads game saved in saveSpot
        public void LoadSavedGame(int saveSpot)
        {
            GameState savedGame = SaveLoad.Load(saveSpot);
            bh.setState(savedGame.balls);
            eh.setState(savedGame.enemies);
            ph.setState(savedGame.player.Deserialize());
            enemyScore = savedGame.cpuScore;
            playerScore = savedGame.playerScore;
            if (paused)
            {
                ResumeGame();
            }
            LoadScreen.SetActive(false);
        }

        //Loads main menu
        public void LoadMainMenu()
        {
            if (paused)
            {
                Time.timeScale = 1;
            }
            LoadScreen.SetActive(false);
            GameResultMenu.SetActive(false);  //Adding this here to avoid creating a new func for end state
            PauseMenu.SetActive(false);
            MainMenu.SetActive(true);
            Time.timeScale = 0;
        }

        //Brings down the pause menu and resumes the game
        public void ResumeGame()
        {
            paused = false;
            Time.timeScale = 1;
            PauseMenu.SetActive(false);
        }

        //Starts the game play
        public void StartGame()
        {
            paused = false;
            MainMenu.SetActive(false);
            enemyScore = 0;
            playerScore = 0;
            Time.timeScale = 1;
            scoreboard.updateEnemyScore(enemyScore);
            scoreboard.updatePlayerScore(playerScore);
            MainMenu.SetActive(false);
            
        }

        public void LoadMenu()
        {
            PauseMenu.SetActive(false);
            MainMenu.SetActive(false);
            LoadScreen.SetActive(true);
        }

        //Display end menu
        public void EndGame()
        {
            Time.timeScale = 0;
            Text resultText = GameResultMenu.GetComponentInChildren<Text>();
            if (playerScore > enemyScore)
            {
                resultText.color = Color.white;
                resultText.text = "Winner!";
            } else 
            {
                //assume we can't end in a draw
                resultText.color = Color.red;
                resultText.text = "Loser!";
            }
            GameResultMenu.SetActive(true);
            paused = true;

        }

        public void PlayHoverSound()
        {
            if (hover.isPlaying) //avoids sound overlap issue
            {
                hover.Stop();
            }
            hover.Play();
        }

        public void PlaySelectSound()
        {
            select.Play();
        }


        //doesn't work -> will work in full build
        public void Exit()
        {
            Application.Quit();
        }

    }

}
