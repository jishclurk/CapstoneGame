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
        public GameObject MenuCamera;
        public GameObject MainMenu;
        public GameObject PauseMenu;

        //Brings up Main Menu
        void Start () {
		    enemyScore = 0;
		    playerScore = 0;
            MenuCamera = Instantiate(MenuCamera);
            LoadMenu();
        }

        //Checks for game state changes (playing and pausing)
        void Update () {
            if (Input.GetKeyDown(KeyCode.Space))
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

        public void Pause()
        {
            Time.timeScale = 0;
            paused = true;
            PauseMenu.SetActive(true);
        }

        public void SaveGame()
        {
            //how though 
        }

        public void LoadSavedGame()
        {
            
        }

        public void LoadMenu()
        {
            if (paused)
            {
                Time.timeScale = 1;
            }
            PauseMenu.SetActive(false);
            MainMenu.SetActive(true);
            MenuCamera.GetComponent<Camera>().enabled = true;
        }


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
            scoreboard.updateEnemyScore(enemyScore);
            scoreboard.updatePlayerScore(playerScore);
            MenuCamera.GetComponent<Camera>().enabled = false;
            MainMenu.SetActive(false);
            Camera.main.enabled = true;
            
        }

        //doesn't work
        public void Exit()
        {
            Application.Quit();
        }

    }

}
