using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {

    public Text loadText;
    public ProgressBar progressBar;
    private float percentComplete;

	// Use this for initialization
	void Start () {
        percentComplete = 0;

        StartCoroutine(LoadScene());
	}
	
	void OnGUI () {
        loadText.text = "Loading... " + percentComplete.ToString() + "%";
        loadText.color = new Color(loadText.color.r, loadText.color.g, loadText.color.b, Mathf.PingPong(Time.time, 1));
        progressBar.SetProgress(percentComplete);
    }

    IEnumerator LoadScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Pong", LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;
        /*
        while (!asyncLoad.isDone)
        {
            percentComplete = asyncLoad.progress;
            yield return null;
        }
        */

        for (int x = 0; x < 100; x++)
        {
            percentComplete = x;
            yield return new WaitForSeconds(0.25f);
        }

        asyncLoad.allowSceneActivation = true;
    }
}
