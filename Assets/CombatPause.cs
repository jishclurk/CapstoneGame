using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPause : MonoBehaviour {

    Canvas PauseScreen;
    public Canvas AbilitiesScreen;
	// Use this for initialization
	void Start () {
        PauseScreen = GetComponent<Canvas>();
        PauseScreen.enabled = false;
        AbilitiesScreen = Instantiate(AbilitiesScreen) as Canvas;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Enable()
    {
        //combatPause = t
        PauseScreen.enabled = true;
        Time.timeScale = 0;
    }

    public void Disable()
    {
        PauseScreen.enabled = false;
        Time.timeScale = 1;

    }
}
