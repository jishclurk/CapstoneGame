using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomeScreenScript : MonoBehaviour {

    private CheckpointManager cpManager;

    // Use this for initialization
    void Start () {  

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CloseScreen()
    {
        gameObject.SetActive(false);
    }

}
