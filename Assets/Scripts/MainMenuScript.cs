using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

	//Logic for main menu
	bool increasingRot = false;

	// Use this for initialization
	GameObject mainMenuCanvas;

	//Set Range elements to not be active
	void Start() {
		mainMenuCanvas = GameObject.Find ("MainMenu");
	}
	
	// Update is called once per frame
	void Update () {
		//Subtle camera rotation
		if (increasingRot) {
			transform.Rotate (new Vector3 (0, 0.05f, 0));
			if (transform.eulerAngles.y > 45.0f && transform.eulerAngles.y < 46.0f)
				increasingRot = false;
		} else {
			transform.Rotate (new Vector3 (0, -0.05f, 0));
			if (transform.eulerAngles.y < 330.0f && transform.eulerAngles.y > 329.0f)
				increasingRot = true;
		}
	
	}

    public void GameStart()
    {
        SceneManager.LoadScene("Pong");
    }

    public void QuitGame()
    {
        Application.Quit();
    }



}
