using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame {
	
    public class GameManager : MonoBehaviour {

	    private int enemyScore;
	    private int playerScore;
	    private int endScore = 5;
        private bool paused = false;

        public Scoreboard scoreboard;
        public GameObject MainMenu;
        public GameObject PauseMenu;
        public GameObject SaveMenu;

        public BallHandler bh;
        public EnemyHandler eh;
        public PlayerHandler ph;

        public AudioSource select;
        public AudioSource hover;

        //Brings up Main Menu
        void Start () {
		    enemyScore = 0;
		    playerScore = 0;
            LoadMenu();
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
        }

        public void ScorePlayerGoal()
        {
            playerScore++;
            scoreboard.updatePlayerScore(playerScore);
        }

        //Pauses game and brings up pause menu
        public void Pause()
        {
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
            GameState currentState = new GameState(bh.saveState(), ph.saveState(), eh.saveState(), playerScore, enemyScore);
            SaveLoad.Save(currentState, saveSpot);
        }

        //Loads game saved in saveSpot
        public void LoadSavedGame(int saveSpot)
        {
            GameState savedGame = SaveLoad.Load(saveSpot);
            bh.setState(savedGame.balls);
            eh.setState(savedGame.enemies);
            ph.setState(savedGame.player);
            enemyScore = savedGame.cpuScore;
            playerScore = savedGame.playerScore;
        }

        //Loads main menu
        public void LoadMenu()
        {
            if (paused)
            {
                Time.timeScale = 1;
            }
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
            MainMenu.SetActive(false);
            enemyScore = 0;
            playerScore = 0;
            Time.timeScale = 1;
            scoreboard.updateEnemyScore(enemyScore);
            scoreboard.updatePlayerScore(playerScore);
            MainMenu.SetActive(false);
            
        }

        public void PlayHoverSound()
        {
            if (hover.isPlaying)
            {
                hover.Stop();
            }
            hover.Play();
        }
        
        public void PlaySelectSound()
        {
            select.Play();
        }

        public void Exit()
        {
            Application.Quit();
        }

    }

}
