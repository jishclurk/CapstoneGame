using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    //where players are placed if the game is loaded at this checkpoint
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

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
    public bool finalCheckpoint;
    public bool startCheckpoint;

    public void Start()
    {
        if (!startCheckpoint)
        {
            CheckPointPopUp = transform.GetChild(0).gameObject.GetComponent<Canvas>();
            SaveAsScreen = transform.GetChild(1).gameObject.GetComponent<Canvas>();

            SaveAsScreen.enabled = false;
            CheckPointPopUp.enabled = false;

            nameInputField = SaveAsScreen.transform.GetChild(5).GetComponent<InputField>();
            nameInputField.onEndEdit.AddListener(delegate { SaveGame(nameInputField.text, true); });
        }
        inTrigger = false;
        firstEnter = true;
        checkpointReached = false;

        gm = GameObject.Find("GameManager").GetComponent<SimpleGameManager>();
        //Debug.Log(gm.level);
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
                tm.ReviveTeam(this);
                autosave();
                firstEnter = false;
                checkpointReached = true;
              
            }
            inTrigger = true;
            CheckPointPopUp.enabled = true;
            if (finalCheckpoint)
            {
                if (objmanager.LevelComplete())
                {
                    CheckPointPopUp.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Level Complete! Press [S] to save and continue/n Press [N] to continue";
                }else
                {
                    CheckPointPopUp.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Need to complete objectives to advance!  Press [S] to save";

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

    private void Update()
    {
        if (inTrigger)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                LanchSaveScreen();
               // CheckPointPopUp.enabled = false;
            }
            if (finalCheckpoint && objmanager.LevelComplete())
            {
                if (Input.GetKeyDown(KeyCode.N))
                {
                    Debug.Log("MOVING TO NEXT LEVEL");
                    Debug.Log(gm.level);
                    gm.nextLevel();
                }
            }

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
        UnPause();
        SceneManager.LoadScene("MainMenu");
    }

    //Pulls up save screen
    public void LanchSaveScreen()
    {
            gm.OnStateChange += Pause;
            gm.SetGameState(GameState.PAUSE);
            SaveAsScreen.enabled = true;
    }

    private void autosave()
    {
        Debug.Log("autosaving");
        SavedState autosave = new SavedState();
        autosave.setFromGameManager();
        autosave.name = "autosave";
        autosave.players = tm.currentState();
        autosave.objectives = objmanager.currentState();
        autosave.checkPoint = checkpointManager.GetCheckPoint(this);
        if (finalCheckpoint && objmanager.LevelComplete())
        {
            autosave.level++;
            autosave.checkPoint = 0;
        }
        SaveLoad.Save(autosave, "autosave");
        gm.autosave = autosave;
    }

    //Sets game name as input in InputFeild, saves the game 
    public void SaveGame(string name, bool newGame)
    {
        Debug.Log("in save game");
        SaveAsScreen.enabled = false;
        SavedState toSave = new SavedState();
        toSave.setFromGameManager();
        toSave.name = name;
        toSave.players = tm.currentState();
        toSave.objectives = objmanager.currentState();
        toSave.checkPoint = checkpointManager.GetCheckPoint(this);
        if (finalCheckpoint && objmanager.LevelComplete()) {
            toSave.level++;
            toSave.checkPoint = 0;
        }
        SaveLoad.Save(toSave,name);
        if (newGame)
        {
            checkpointManager.UpdateButtons(name);
        }
        Debug.Log("saved");
        progressGame();
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

    public void progressGame()
    {
        SaveAsScreen.enabled = false;
        gm.OnStateChange += UnPause;
        gm.SetGameState(GameState.PLAY);
        if (finalCheckpoint && objmanager.LevelComplete())
        {
            gm.nextLevel();
        }
    }
}
