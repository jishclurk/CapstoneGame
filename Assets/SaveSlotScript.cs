using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotScript : MonoBehaviour {

    private Canvas saveScreen;
    private TeamManager tm;
    private ObjectiveManager objmanager;
    private SimpleGameManager gm;

    public int checkpoint;
    public bool finalCheckpoint;
    private SaveButton[] slots = new SaveButton[5];

    // Use this for initialization
    void Start () {
        saveScreen = GetComponent<Canvas>();
        tm = GameObject.Find("TeamManager").gameObject.GetComponent<TeamManager>();
        objmanager = GameObject.Find("ObjectiveManager").GetComponent<ObjectiveManager>();
        gm = GameObject.Find("GameManager").gameObject.GetComponent<SimpleGameManager>();

        for (int i = 0; i<slots.Length; i++)
        {
            slots[i] = transform.FindChild("slot" + (i + 1).ToString()).gameObject.GetComponent<SaveButton>();
            slots[i].slotNumber = i + 1;
            SaveLoad.setSaveButton(slots[i]);
        }
        saveScreen.enabled = false;
    }

    public void enableSaveScreen()
    {
        saveScreen.enabled = true;
    }

    public void disableSaveScreen()
    {
        saveScreen.enabled = false;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
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
    public void Exit()
    {
        disableSaveScreen();
        gm.OnStateChange += UnPause;
        gm.SetGameState(GameState.PLAY);
    }

    public void SaveGame(int slot)
    {
        Debug.Log("in save game");
     //   saveScreen.enabled = false;
        SavedState toSave = new SavedState();
        toSave.setFromGameManager();
        toSave.players = tm.currentState();
        toSave.objectives = objmanager.currentState();
        toSave.checkPoint = checkpoint;
        toSave.saveSlot = slot;
        toSave.level = gm.level;
        toSave.date = System.DateTime.Now.ToString();
        if (finalCheckpoint && objmanager.LevelComplete())
        {
            toSave.level++;
            toSave.checkPoint = 0;
        }
        SaveLoad.Save(toSave, slot);
        SaveLoad.setSaveButton(slots[slot-1]);
        SavedSuccessfully();
    }

    private void SavedSuccessfully()
    {
        Debug.Log("Saved Succesfully");
        
        //show message
        //close save screen;
        //unpause

    }
}
