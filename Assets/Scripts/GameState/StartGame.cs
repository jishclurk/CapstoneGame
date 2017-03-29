using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SimpleGameManager gm = GameObject.Find("GameManager").GetComponent<SimpleGameManager>();
        gm.OnStateChange += LoadMainMenu;
		gm.SetGameState (GameState.MAIN_MENU);
	}

	public void LoadMainMenu(){
		SceneManager.LoadScene ("MainMenu");
	}

}
