using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsolePair : MonoBehaviour, ICircuitPiece {

	Console[] consoles;
	GameObject consoleGUIObject;
	ConsoleGUI cG;
	bool solved;

	// Use this for initialization
	void Start () {
		consoles = this.transform.GetComponentsInChildren<Console>();
		consoleGUIObject = transform.FindChild ("ConsoleGUI").gameObject;
		consoleGUIObject.gameObject.SetActive (false);
		cG = consoleGUIObject.GetComponentInChildren<ConsoleGUI> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (consoles [0].Output () && consoles [1].Output ()) {
			consoleGUIObject.SetActive (true);
		} else {
			consoleGUIObject.SetActive (false);
		}
	}

	public bool Output(){
		return cG.Output ();

	}

	public bool ConsoleOn(){
		return consoles [0].Output () && consoles [1].Output ();

	}

	public Transform GetTransform(){
		return transform;
	}

	public void Lock(){
		solved =true;
	}
}
