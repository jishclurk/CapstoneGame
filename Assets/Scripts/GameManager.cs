using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
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
        public GameObject GameResultMenu;

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
            GameResultMenu.SetActive(false);  //Adding this here to avoid creating a new func for end state
            PauseMenu.SetActive(false);
            MainMenu.SetActive(true);
            Time.timeScale = 0;
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
            Time.timeScale = 1;
            scoreboard.updateEnemyScore(enemyScore);
            scoreboard.updatePlayerScore(playerScore);
            MainMenu.SetActive(false);
            
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
