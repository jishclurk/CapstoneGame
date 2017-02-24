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
	Gen_Collide trigger;
	private bool solved;


	// Use this for initialization
	void Start () {
		input = GetComponentsInChildren<ICircuitPiece> ();
		connector = this.gameObject.transform.Find ("Out Connector").GetComponent<MeshRenderer> ();
		genSpot = this.gameObject.transform.Find ("Spot Light").GetComponent<Light> ();
		turnedOn = false;
		trigger = this.gameObject.transform.Find ("ColliderEq_2").GetComponent<Gen_Collide> ();
		solved = false;
	}

	// Update is called once per frame
	void Update () {
		turnedOn = trigger.triggered;

		if (!solved) {
			if (turnedOn) {
				if (this.Output ()) {
					connector.material = genActive;
					input [1].Lock ();
					Lock ();

				} else {
					connector.material = genInactive;
				}
				genSpot.intensity = 100;
			} else {
				genSpot.intensity = 0;
			}
		}

		//} else {


		//}
	}

	public bool Output(){
		return input[1].Output ();
	}

	public bool isSending(){
		return Output () && turnedOn;
	}

	public void Lock(){
		solved = true;
	}

	public Transform GetTransform(){
		return this.transform;
	}


}
