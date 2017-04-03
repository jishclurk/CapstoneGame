using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

    public KeyCode[] AbilityBindings = new KeyCode[4];

    public float volume;

    public float resY;
    public float resX;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
