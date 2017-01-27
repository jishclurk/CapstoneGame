using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

namespace CapstoneGame {
	
    public class GameManager : MonoBehaviour {

	    private int enemyScore;
	    private int playerScore;
	    private int endScore = 2;
        private bool paused = true;
        private bool gameOver = false;
        private GameState test1;
        private GameState test2;
        private GameState test3;

        public Scoreboard scoreboard;
        public GameObject MainMenu;
        public GameObject PauseMenu;
        public GameObject GameResultMenu;
        public GameObject SaveMenu;
        public GameObject LoadScreen;

        public BallHandler bh;
        public EnemyHandler eh;
        public PlayerHandler ph;

        public Vector3 defaultPlayerPosition;
        public Vector3 defaultEnemyPosition;

        public AudioSource select;
        public AudioSource hover;

        //Brings up Main Menu
        void Start () {
            Debug.Log(Application.dataPath);
		    enemyScore = 0;
		    playerScore = 0;
            LoadMainMenu();
            SetUpTestStates();
        }

        private void SetUpTestStates()
        {
            //test 1, cpu is about to win
            test1 = new GameState();

            BallState ball1 = new BallState();
            SerializableVector3 position = new SerializableVector3();
            SerializableVector3 velocity = new SerializableVector3();
            position.setVector(new Vector3(0, 0, 0));
            velocity.setVector(new Vector3(-8f, .5f, 0));
            ball1.velocity = velocity;
            ball1.position = position;
            List<BallState> balls = new List<BallState>();
            balls.Add(ball1);

            SerializableVector3 player = new SerializableVector3();
            player.setVector(defaultPlayerPosition);
            SerializableVector3 enemy = new SerializableVector3();
            enemy.setVector(defaultEnemyPosition);
            List<SerializableVector3> enemies = new List<SerializableVector3>();
            enemies.Add(enemy);

            test1.balls = balls;
            test1.cpuScore = endScore;
            test1.playerScore = 0;
            test1.player = player;
            test1.enemies = enemies;

            //test 2, player is winning, ball is coming towards them and enemy is not centered
            test2 = new GameState();
            position.setVector(new Vector3(10, 0, 0));
            velocity.setVector(new Vector3(8f, .5f, 0));
            ball1.velocity = velocity;
            ball1.position = position;
            balls = new List<BallState>();
            balls.Add(ball1);

            player = new SerializableVector3();
            player.setVector(defaultPlayerPosition);
            enemy = new SerializableVector3();
            enemy.setVector(defaultEnemyPosition);
            enemy.y = enemy.y - 5;
            enemies = new List<SerializableVector3>();
            enemies.Add(enemy);

            test2.balls = balls;
            test2.cpuScore = 0;
            test2.playerScore = endScore;
            test2.player = player;
            test2.enemies = enemies;

            //test 3, universe point! ball headed towards enemy who is out of place, player is also out of place
            test3 = new GameState();
            position.setVector(new Vector3(0, 0, 0));
            velocity.setVector(new Vector3(-8f, .5f, 0));
            ball1.velocity = velocity;
            ball1.position = position;
            balls = new List<BallState>();
            balls.Add(ball1);

            player = new SerializableVector3();
            player.setVector(defaultPlayerPosition);
            player.y = player.y - 5;
            enemy = new SerializableVector3();
            enemy.setVector(defaultEnemyPosition);
            enemy.y = enemy.y + 5;
            enemies = new List<SerializableVector3>();
            enemies.Add(enemy);

            test3.balls = balls;
            test3.cpuScore = endScore;
            test3.playerScore = endScore;
            test3.player = player;
            test3.enemies = enemies;

        }

        //Checks for game state changes (playing, pausing, and test states)
        void Update () {
            if (!MainMenu.activeSelf && Input.GetKeyDown(KeyCode.Space) && !gameOver)
            {
                if (paused)
                {
                    ResumeGame();

                }else
                {
                    Pause();
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                LoadTestState(test1);

            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                LoadTestState(test2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                LoadTestState(test3);

            }

        }

        void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Application.CaptureScreenshot(string.Format("screen_{0}.png", System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")));
            }
        }

        public void ScoreEnemyGoal()
        {
            enemyScore++;
            scoreboard.updateEnemyScore(enemyScore);
            if (enemyScore > endScore) 
            {
                EndGame();
            }
        }

        public void ScorePlayerGoal()
        {
            playerScore++;
            scoreboard.updatePlayerScore(playerScore);
            if(playerScore > endScore)
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
            SaveMenu.SetActive(false);
            PauseMenu.SetActive(true);
        }


        private void ResetGame()
        {
            playerScore = 0;
            enemyScore = 0;
            ph.Restart();
            bh.Restart();
            eh.Restart();
            scoreboard.updateAll(enemyScore, playerScore);
        }

        //Loads game saved in saveSpot
        public void LoadSavedGame(int saveSpot)
        {
            try
            {
                GameState savedGame = SaveLoad.Load(saveSpot);
                bh.setState(savedGame.balls);
                eh.setState(savedGame.enemies);
                ph.setState(savedGame.player.Deserialize());
                enemyScore = savedGame.cpuScore;
                playerScore = savedGame.playerScore;
                scoreboard.updateAll(enemyScore, playerScore);
                if (paused)
                {
                    ResumeGame();
                }
                LoadScreen.SetActive(false);

            }
            catch (Exception e)
            {

            }

        }

        //Disables all menus
        private void DisableMenus()
        {
            MainMenu.SetActive(false);
            PauseMenu.SetActive(false);
            GameResultMenu.SetActive(false);
            SaveMenu.SetActive(false);
            LoadScreen.SetActive(false);
        }

        private void LoadTestState(GameState testState)
        {
            //paused = false;
            DisableMenus();
            bh.setState(testState.balls);
            eh.setState(testState.enemies);
            ph.setState(testState.player.Deserialize());
            enemyScore = testState.cpuScore;
            playerScore = testState.playerScore;
            scoreboard.updateAll(enemyScore, playerScore);
            if (paused)
            {
                ResumeGame();
            }
            LoadScreen.SetActive(false);
        }

        //Loads main menu
        public void LoadMainMenu()
        {
            ResetGame();
            scoreboard.clearAll();
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
            gameOver = false;
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
            gameOver = true;
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
