
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{

    public Canvas CheckPointSceen;
	public Canvas SaveAsScreen;
    private Button yes;
    private Button no;
    private Button exit;
    private InputField name;

    public bool checkpointReached;
	private Collider col;
	private SimpleGameManager gm;


    public void Start()
    {
        CheckPointSceen = Instantiate(CheckPointSceen) as Canvas;
        CheckPointSceen = CheckPointSceen.GetComponent<Canvas>();

		SaveAsScreen = Instantiate (SaveAsScreen) as Canvas;
		SaveAsScreen = SaveAsScreen.GetComponent<Canvas> ();

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

    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGERED");
        if (other.gameObject.tag.Equals("Player"))
        {
            checkpointReached = true;

			gm.OnStateChange += Pause;
            Debug.Log("here");
			gm.SetGameState (GameState.PAUSE);
			//gm.checkpoint++;
			DisableCollider ();
            Debug.Log("878787");
			yes.onClick.AddListener(Save);
            no.onClick.AddListener(closeCheckpointScreen);
			exit.onClick.AddListener(ToMain);

            CheckPointSceen.enabled = true;
            Debug.Log("asdfasdf");
        }
    }

	public void ToMain(){
        Debug.Log("setting state change to load main menu");
		gm.OnStateChange += loadMainMenu;
		gm.SetGameState (GameState.MAIN_MENU);
	}

	private void loadMainMenu(){
		SceneManager.LoadScene ("MainMenu");
	}

	public void DisableCollider(){
		Collider col = gameObject.GetComponent<Collider>();
		col.enabled = false;
	}


    public void Save() {
        CheckPointSceen.enabled = false;
        Debug.Log("in save");
        if (gm.hasBeenSaved) {
            
            //SaveLoad.Save
        } else {
            Debug.Log(name);
            SaveAsScreen.enabled = true;
            name.onEndEdit.AddListener(delegate { setNameAndSave(name); });
        }
    }

    //Sets game name as input in InputFeild, saves the game 
    private void setNameAndSave(InputField gameName)
    {
        Debug.Log("set name and save");
        gm.name = gameName.text;
        SaveAsScreen.enabled = false;
        SavedState toSave = new SavedState();
        toSave.setFromGameManager();
        SaveLoad.Save(toSave, gameName.text);
        Debug.Log("saved");
        progressGame();
        //
    }

	public void Pause(){
		Time.timeScale = 0;
	}

    public void closeCheckpointScreen()
    {
        Debug.Log("000000");
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
        Debug.Log("33333");

        gm.OnStateChange += gm.NextCheckPointOrLevel;
        gm.SetGameState(GameState.PLAY);
    }
}
