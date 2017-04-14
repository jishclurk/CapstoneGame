using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomeScreenScript : MonoBehaviour {

    private CheckpointManager cpManager;

    // Use this for initialization
    void Start () {  
        cpManager = GameObject.Find("CheckpointManager").GetComponent<CheckpointManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
