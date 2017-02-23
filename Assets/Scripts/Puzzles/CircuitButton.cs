using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitButton : MonoBehaviour, ICircuitPiece {

	ButtonScript button;
	MeshRenderer connector;
	public Material activeGreen;
	public Material inactiveWhite;
	private bool solved;
	// Use this for initialization
	void Start () {
		button = GetComponentInChildren<ButtonScript> ();
		connector = this.gameObject.transform.Find ("Out Connector").GetComponent<MeshRenderer> ();
	}

	// Update is called once per frame
	void Update () {
//		if (Output ()) {
//			connector.material.Lerp(inactiveWhite, activeGreen, 1.0f);
//		} else {
//			connector.material.Lerp(activeGreen,inactiveWhite,  1.0f);
//		}
		if (!solved) {
			if (this.Output ()) {
				connector.material = activeGreen;
			} else {
				connector.material = inactiveWhite;
			}
		}
	}

	public bool Output(){

		return this.button.Pressed ();
	}

	public Transform GetTransform(){
		return this.transform;
	}

	public void Lock(){
		solved = true;
		button.Disable ();
	}
}
