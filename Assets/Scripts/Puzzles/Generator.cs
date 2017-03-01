using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour,ICircuitPiece {

	private ICircuitPiece[] input;
	MeshRenderer connector;
	Light genSpot;
	Material genActive;
	Material genInactive;
	bool turnedOn;
	Gen_Collide trigger;
	private bool solved;
	float t = 1.0f;
	float minimum = -0.75f;
	float maximum = 0.75f;


	// Use this for initialization
	void Start () {
		input = GetComponentsInChildren<ICircuitPiece> ();
		connector = this.gameObject.transform.Find ("Out Connector").GetComponent<MeshRenderer> ();
		genSpot = this.gameObject.transform.Find ("Spot Light").GetComponent<Light> ();
		turnedOn = false;
		trigger = this.gameObject.transform.Find ("ColliderEq_2").GetComponent<Gen_Collide> ();
		solved = false;
		genActive = Resources.Load ("Materials/Green_Beam") as Material;
		genInactive = Resources.Load("Materials/White_Beam") as Material;
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

		connector.material.mainTextureOffset = new Vector2(0.0f, Mathf.Lerp(minimum,maximum,t));
		// .. and increate the t interpolater
		t += 0.75f * Time.deltaTime;
		if (t > 1.0f){
			t = 0.0f;
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
