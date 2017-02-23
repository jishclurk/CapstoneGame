using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleGUI : MonoBehaviour, ICircuitPiece {


	Text[] errorText;
	SpriteRenderer[] flickeringImages;
	PuzzleManager pm;
	AudioSource sound;

	float flickerTime = 0.2f;
	float cTime;
	bool flick;
	bool played;

	// Use this for initialization
	void Start () {
		errorText = GetComponentsInChildren<Text> ();
		flickeringImages = transform.GetComponentsInChildren<SpriteRenderer> ();
		pm = transform.GetComponentInChildren<PuzzleManager> ();
		cTime = 0;
		flick = false;
		sound = GetComponent<AudioSource>();
		played = false;

	}
	
	// Update is called once per frame
	void Update () {
		if (pm.Output ()) {
				errorText [0].color = Color.cyan;
				errorText [1].color = Color.cyan;
				errorText [0].text = "ACCESS";
				errorText [1].text = "GRANTED";
				if (!played) {
					sound.Play ();
					played = true;
				}
		} else {
			foreach (Text t in errorText) {
				if (cTime < flickerTime) {
					if (flick) {
						t.color = Color.clear;
					} else {
						t.color = Color.red;
					}

				} else {
					flick = !flick;
					cTime = 0;

				}
			}
			cTime += Time.deltaTime;

		}
	}

	public bool Output(){
		return pm.Output ();
	}

	public Transform GetTransform(){

		return transform;

	}
}
