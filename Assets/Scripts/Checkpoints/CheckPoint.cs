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

    public Canvas CheckPointSceen;
    public Canvas SaveAsScreen;
    private Button yes;
    private Button no;
    private Button exit;
    private InputField name;
    private TeamManager tm;

    public bool checkpointReached;
    private Collider col;
    private SimpleGameManager gm;
    private CheckpointManager manager;

    private bool playerHasHit;


    public void Start()
    {
        playerHasHit = false;

        CheckPointSceen = Instantiate(CheckPointSceen) as Canvas;
        CheckPointSceen = CheckPointSceen.GetComponent<Canvas>();

        SaveAsScreen = Instantiate(SaveAsScreen) as Canvas;
        SaveAsScreen = SaveAsScreen.GetComponent<Canvas>();

        SaveAsScreen.enabled = false;
        CheckPointSceen.enabled = false;
        checkpointReached = false;

        yes = CheckPointSceen.transform.GetChild(2).GetComponent<Button>();
        no = CheckPointSceen.transform.GetChild(3).GetComponent<Button>();
        exit = CheckPointSceen.transform.GetChild(4).GetComponent<Button>();
        name = SaveAsScreen.transform.GetChild(0).GetComponent<InputField>();

        col = gameObject.GetComponent<Collider>();
        col.enabled = true;

        gm = SimpleGameManager.Instance;
        tm = GameObject.Find("TeamManager").gameObject.GetComponent<TeamManager>();

        manager = transform.parent.gameObject.GetComponent<CheckpointManager>();

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") && (!other.isTrigger) && !playerHasHit)
        {
            playerHasHit = true;
            Debug.Log("TRIGGERED CHECKPOINT");
            checkpointReached = true;
            gm.OnStateChange += Pause;
            gm.SetGameState(GameState.PAUSE);
            DisableCollider();
            yes.onClick.AddListener(Save);
            no.onClick.AddListener(closeCheckpointScreen);
            exit.onClick.AddListener(ToMain);

            CheckPointSceen.enabled = true;

            manager.UpdateCheckpoints();
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

    public void DisableCollider()
    {
        Collider col = gameObject.GetComponent<Collider>();
        col.enabled = false;
    }


    public void Save()
    {
        CheckPointSceen.enabled = false;
        Debug.Log("in save");
        if (gm.hasBeenSaved)
        {

            SaveGame();
        }
        else
        {
            Debug.Log(name);
            SaveAsScreen.enabled = true;
            name.onEndEdit.AddListener(delegate { setName(name); });
        }
    }

    private void setName(InputField name)
    {
        gm.name = name.text;
        SaveGame();
    }

    //Sets game name as input in InputFeild, saves the game 
    private void SaveGame()
    {
        Debug.Log("set name and save");
        //gm.name = gameName.text;
        SaveAsScreen.enabled = false;

        SavedState toSave = new SavedState();
        toSave.setFromGameManager();
        toSave.players = tm.currentState();

        SaveLoad.Save(toSave, gm.name);
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

    //Closes the checkpoint scrren and starts the game
    public void closeCheckpointScreen()
    {
        CheckPointSceen.enabled = false;
        progressGame();
    }

    public void openCheckPointScreen()
    {
        SaveAsScreen.enabled = false;
        CheckPointSceen.enabled = true;
    }

    private void progressGame()
    {
        gm.OnStateChange += UnPause;
        gm.SetGameState(GameState.PLAY);
    }
}
