using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameScript : MonoBehaviour {

    public Image BlackScreen;
    public Image FinalScreen;

    public float fadeToBlackTime = 4;
    public float fadeToPictureTime = 2;
    public float timeOnScreen = 10;

    private float timer = 0;
    private bool fadeToBlackComplete = false;

	// Use this for initialization
	void Start () {
        BlackScreen.CrossFadeAlpha(0, 0, true);
        FinalScreen.CrossFadeAlpha(0, 0, true);
        BlackScreen.CrossFadeAlpha(1, fadeToBlackTime, true);
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if (!fadeToBlackComplete && timer > fadeToBlackTime)
        {
            fadeToBlackComplete = true;
            FinalScreen.CrossFadeAlpha(1, fadeToPictureTime, true);
        }
        else if (fadeToBlackComplete && timer > (fadeToBlackTime + fadeToPictureTime + timeOnScreen))
        {
            SceneManager.LoadSceneAsync("MainMenu");
        }
	}
}
