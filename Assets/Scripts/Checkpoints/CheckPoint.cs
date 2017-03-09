using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    //where players are placed if the game is loaded at this checkpoint
    public Vector3 player1;
    public Vector3 player2;
    public Vector3 player3;
    public Vector3 player4;


    private Canvas CheckPointPopUp;
    private Canvas SaveAsScreen;
    private Canvas LevelComplete;
    private InputField nameInputField;

    //managers
    private TeamManager tm;
    private ObjectiveManager objmanager;
    private CheckpointManager checkpointManager;
    private SimpleGameManager gm;

    public bool checkpointReached;
    private bool inTrigger;
    private bool firstEnter;


    public void Start()
    {
        inTrigger = false;
        firstEnter = true;
        checkpointReached = false;

        CheckPointPopUp = transform.GetChild(0).gameObject.GetComponent<Canvas>();
        SaveAsScreen = transform.GetChild(1).gameObject.GetComponent<Canvas>();

        SaveAsScreen.enabled = false;
        CheckPointPopUp.enabled = false;

        nameInputField = SaveAsScreen.transform.GetChild(6).GetComponent<InputField>();

        gm = SimpleGameManager.Instance;
        Debug.Log(gm.level);
        tm = GameObject.Find("TeamManager").gameObject.GetComponent<TeamManager>();
        checkpointManager = transform.parent.gameObject.GetComponent<CheckpointManager>();
        objmanager = GameObject.Find("ObjectiveManager").GetComponent<ObjectiveManager>();

    }

    //called when player is 'close' to the checkpoint
    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag.Equals("Player") && (!other.isTrigger) && !inTrigger)
        {
            if (firstEnter)
            {
                gm.AutoSave();
                checkpointManager.UpdateCheckpoints();
                firstEnter = false;
                checkpointReached = true;
            }
            inTrigger = true;
            CheckPointPopUp.enabled = true;
            if (checkpointManager.levelCompleted)
            {
                CheckPointPopUp.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Level Complete! Press [S] to save and continue/n Press [N] to continue";

            }
           
        }
    }

    private void Update()
    {
        if (inTrigger)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                LanchSaveScreen();
                CheckPointPopUp.enabled = false;
            }
            if (checkpointManager.levelCompleted)
            {
                if (Input.GetKeyDown(KeyCode.N))
                {
                    Debug.Log(gm.level);
                    gm.nextLevel();
                }
            }

        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (inTrigger)
        {
            Debug.Log("EXITED TRIGGER");
            CheckPointPopUp.enabled = false;
            inTrigger = false;
        }
     
    }

    public void ToMain()
    {
        Debug.Log("setting state change to load main menu");
        gm.OnStateChange += loadMainMenu;
        gm.SetGameState(GameState.MAIN_MENU);
    }

    private void loadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //Pulls up save screen
    public void LanchSaveScreen()
    {
        Debug.Log("in save");
            gm.OnStateChange += Pause;
            gm.SetGameState(GameState.PAUSE);
            Debug.Log(nameInputField);
            SaveAsScreen.enabled = true;
            nameInputField.onEndEdit.AddListener(delegate { SaveGame(nameInputField.text); });
    }

    //Sets game name as input in InputFeild, saves the game 
    public void SaveGame(string name)
    {

        SaveAsScreen.enabled = false;
        SavedState toSave = new SavedState();
        toSave.setFromGameManager();
        toSave.name = name;
        toSave.players = tm.currentState();
        toSave.objectives = objmanager.currentState();
        if (checkpointManager.levelCompleted) {
            toSave.level++;
            toSave.checkPoint = 0;
        }
        SaveLoad.Save(toSave,name);
        Debug.Log("saved");
        progressGame();
        //
    }

    //Freezes game
    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
    }

    ////Closes the checkpoint scrren and starts the game
    //public void closeCheckpointScreen()
    //{
    //   // CheckPointSceen.enabled = false;
    //    progressGame();
    //}

    //public void openCheckPointScreen()
    //{
    //    SaveAsScreen.enabled = false;
    //   // CheckPointSceen.enabled = true;
    //}

    private void progressGame()
    {
        gm.OnStateChange += UnPause;
        gm.SetGameState(GameState.PLAY);
        if (checkpointManager.levelCompleted)
        {
            gm.nextLevel();
        }
    }
}
