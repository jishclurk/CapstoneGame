using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopPiece : MonoBehaviour {

	float rotationValue;
	public int[] values;
	float speed =0.3f;

	PuzzleManager gm;
	// Use this for initialization
	void Start () {
		gm = transform.GetComponentInParent<PuzzleManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.localRotation.eulerAngles.z != rotationValue) {
			transform.localRotation = Quaternion.Lerp (transform.localRotation, Quaternion.Euler (0, 0, rotationValue), speed);
		}
	}

	void OnMouseDown(){
		if (!(gm.puzzle.curValue == gm.puzzle.winValue)) {
			int difference = -gm.QuickSweep ((int)transform.localPosition.x, (int)transform.localPosition.y);

			RotatePiece ();

			difference += gm.QuickSweep ((int)transform.localPosition.x, (int)transform.localPosition.y);

			gm.puzzle.curValue += difference;

		} 
		if (gm.puzzle.curValue == gm.puzzle.winValue) {
			gm.Win ();
		}

	}

	public void RotatePiece(){
		rotationValue += 90;

		if (rotationValue == 360) {
			rotationValue = 0;
		}

		RotateValues ();

	}

	public void RotateValues(){

		int temp = values [0];

		for (int i = 0; i < values.Length-1; i++) {
			values [i] = values [i + 1];
		}
		values [3] = temp;
	}
}
