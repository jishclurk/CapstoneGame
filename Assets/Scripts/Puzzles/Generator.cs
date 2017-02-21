using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour,ICircuitPiece {

	private ICircuitPiece[] input;
	MeshRenderer connector;
	Light genSpot;
	public Material genActive;
	public Material genInactive;
	bool turnedOn;

	// Use this for initialization
	void Start () {
		input = GetComponentsInChildren<ICircuitPiece> ();
		connector = this.gameObject.transform.Find ("Out Connector").GetComponent<MeshRenderer> ();
//		genSpot = this.gameObject.transform.Find ("Spot Light").GetComponent<Light> ();
//		turnedOn = false;

	}
	
	// Update is called once per frame
	void Update () {
		//if (turnedOn) {
			if (Output ()) {
				connector.material = genActive;
			} else {
				connector.material = genInactive;
			}

		//} else {


		//}
	}

	public bool Output(){
		return input[1].Output ();
	}
}
