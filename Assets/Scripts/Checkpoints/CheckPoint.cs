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

    public GameObject SaveScreenGameObject;

    private Canvas CheckPointPopUp;
    private SaveSlotScript SaveScreen;
    private Canvas LevelComplete;
   // private InputField nameInputField;

    //managers
    private TeamManager tm;
    private ObjectiveManager objmanager;
    private CheckpointManager checkpointManager;
    private SimpleGameManager gm;
    private Text autosaveText;

    public bool checkpointReached;
    private bool inTrigger;
    public bool firstEnter = true;
    public bool finalCheckpoint;
    public bool startCheckpoint;

    public void Start()
    {
        if (!startCheckpoint)
        {
            CheckPointPopUp = transform.Find("cp canvas").gameObject.GetComponent<Canvas>();
            autosaveText = CheckPointPopUp.transform.Find("autosavedText").gameObject.GetComponent<Text>();
            autosaveText.enabled = false;

            CheckPointPopUp.enabled = false;
            SaveScreen = SaveScreenGameObject.GetComponent<SaveSlotScript>();

        }
        inTrigger = false;
        checkpointReached = false;

        gm = GameObject.Find("GameManager").GetComponent<SimpleGameManager>();
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
                autosaveText.enabled = true;
                tm.ReviveTeam(this);
                autosave();
                firstEnter = false;
                checkpointReached = true;

            }else
            {
                autosaveText.enabled = false;
            }
            inTrigger = true;
            CheckPointPopUp.enabled = true;
            if (finalCheckpoint)
            {
                if (objmanager.LevelComplete())
                {
                    CheckPointPopUp.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Level Complete! Press [S] to save and continue Press [N] to continue";
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
            //Debug.Log("EXITED TRIGGER");
            CheckPointPopUp.enabled = false;
            inTrigger = false;
            SaveScreen.enabled = false;
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
                    CheckPointPopUp.enabled = false;
                    //Debug.Log("MOVING TO NEXT LEVEL");
                    //Debug.Log(gm.level);
                    gm.nextLevel();
                }
            }

        }
    }

    public void ToMain()
    {
        //Debug.Log("setting state change to load main menu");
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
        SaveScreen.checkpoint = checkpointManager.GetCheckPoint(this);
            SaveScreen.enableSaveScreen();
    }

    private void autosave()
    {
        Debug.Log("autosaving");
        SavedState autosave = new SavedState();
        autosave.setFromGameManager();
       // autosave.name = "autosave";
        autosave.players = tm.currentState();
        autosave.objectives = objmanager.currentState();
        autosave.checkPoint = checkpointManager.GetCheckPoint(this);
        autosave.date = System.DateTime.Now.ToString();
        autosave.saveSlot = 6;
        if (finalCheckpoint && objmanager.LevelComplete())
        {
            autosave.level++;
            autosave.checkPoint = 0;
        }
        SaveLoad.Save(autosave, 6);
        gm.autosave = autosave;
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

    public void progressGame()
    {
        SaveScreen.disableSaveScreen();
        gm.OnStateChange += UnPause;
        gm.SetGameState(GameState.PLAY);
        if (finalCheckpoint && objmanager.LevelComplete())
        {
            gm.nextLevel();
        }
    }
}
