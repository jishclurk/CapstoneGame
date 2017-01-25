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

        private enum State { Load, Play, Pause, Menu}
        private State state = State.Menu;

        public Scoreboard scoreboard;
        public GameObject MenuCamera;
        public GameObject MainMenu;
        public GameObject PauseMenu;
        public BallHandler ballHandler;

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
                    new Resume(this).Execute();
                }else
                {
                    paused = true;
                    new Pause(this).Execute();
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

        public void LoadPauseScreen()
        {
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
            MainMenu.SetActive(true);
            MenuCamera.GetComponent<Camera>().enabled = true;

        }
        
        public void ResumeGame()
        {
            PauseMenu.SetActive(false);
        }

        public void StartGame()
        {
            MainMenu.SetActive(false);
            enemyScore = 0;
            playerScore = 0;
            scoreboard.updateEnemyScore(enemyScore);
            scoreboard.updatePlayerScore(playerScore);
           // ballHandler.Reset();
            MenuCamera.GetComponent<Camera>().enabled = false;
            MainMenu.SetActive(false);

            Camera.main.enabled = true;
            
        }

    }

}
