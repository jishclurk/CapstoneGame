using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitButton : MonoBehaviour, ICircuitPiece {

	ButtonScript button;
	MeshRenderer connector;
	Material activeGreen;
	Material inactiveWhite;
	private bool solved;
	float cTime;
	bool flick;
	float t = 1.0f;
	float minimum = -0.75f;
	float maximum = 0.75f;
	// Use this for initialization


	void Start () {
		connector = this.gameObject.transform.Find ("Out Connector").GetComponent<MeshRenderer> ();
		button = GetComponentInChildren<ButtonScript> ();
		activeGreen = Resources.Load ("Materials/Green_Beam") as Material;
		inactiveWhite = Resources.Load("Materials/White_Beam") as Material;
	}

	// Update is called once per frame
	void Update () {
//		if (Output ()) {
//			connector.material.Lerp(inactiveWhite, activeGreen, 1.0f);
//		} else {
//			connector.material.Lerp(activeGreen,inactiveWhite,  1.0f);
//		}
//		if (!solved) {
			if (this.Output ()) {
				connector.sharedMaterial = activeGreen;
			} else {
			connector.sharedMaterial = inactiveWhite;
			}


//		} else {
		connector.sharedMaterial.mainTextureOffset = new Vector2 (0.0f, Mathf.Lerp (minimum, maximum, t));
			// .. and increate the t interpolater
			t += 0.75f * Time.deltaTime;

			//			// now check if the interpolator has reached 1.0
			//			// and swap maximum and minimum so game object moves
			//			// in the opposite direction.
			if (t > 1.0f) {
				t = 0.0f;
			}
//		}
	}
		

	public bool Output(){

		return this.button.Pressed ();
	}

	public Transform GetTransform(){
		return this.transform;
	}

	public void Lock(){
		//solved = true;
		button.Disable ();
	}
		
}

public enum AngleType{
	NORTH, SOUTH, EAST, WEST

}
