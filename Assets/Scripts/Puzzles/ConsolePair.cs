using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsolePair : MonoBehaviour, ICircuitPiece {

	Console[] consoles;
	GameObject consoleGUIObject;
	ConsoleGUI cG;
	ConsoleGUITime cGT;
	bool solved;

	// Use this for initialization
	void Start () {
		consoles = this.transform.GetComponentsInChildren<Console>();
		consoleGUIObject = transform.FindChild ("ConsoleGUI").gameObject;
		consoleGUIObject.gameObject.SetActive (false);
		cG = consoleGUIObject.GetComponentInChildren<ConsoleGUI> ();
		cGT = consoleGUIObject.GetComponentInChildren<ConsoleGUITime> ();
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
		return cG.Output () || cGT.Output();

	}

	public bool ConsoleOn(){
		return consoles [0].Output () && consoles [1].Output ();

	}

	public Transform GetTransform(){
		return transform;
	}

	public void Lock(){
		solved =true;
		consoles [0].Lock ();
		consoles [1].Lock ();
	}

	public void Solve(){
		consoles [0].Solve ();
		consoles [1].Solve ();
		cG.Solve ();
	}
}
