using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleGUITime : MonoBehaviour, ICircuitPiece {


	Text[] errorText;
	SpriteRenderer[] flickeringImages;
	PuzzleManager pm;
	AudioSource sound;
	public bool minuteSolved;

	float flickerTime = 0.2f;
	float cTime;
	bool flick;
	bool played;
	bool solved;

	// Use this for initialization
	void Start () {
		errorText = GetComponentsInChildren<Text> ();
		flickeringImages = transform.GetComponentsInChildren<SpriteRenderer> ();
		pm = transform.GetComponentInChildren<PuzzleManager> ();
		cTime = 0;
		flick = false;
		sound = GetComponent<AudioSource>();
		played = false;
		solved = false;

	}

	// Update is called once per frame
	void Update () {
		int hour = System.DateTime.Now.Hour % 12;
		int min = System.DateTime.Now.Minute;
		if (hour == 0) {
			hour = 12;
		}
		if (pm.Output ()) {
			errorText [0].color = Color.cyan;
			errorText [2].color = Color.cyan;
			errorText [0].text = errorText[2].text = hour+" :";
			//errorText [2].text = "GRANTED";
			if (!minuteSolved) {
				errorText [1].text = errorText [3].text =	"" + Random.Range (0, 6) + Random.Range (0, 10);
			} else {
				errorText [1].color = Color.cyan;
				errorText [3].color = Color.cyan;
				if (min < 10) {
					errorText [1].text = errorText [3].text = "0" + min;
				} else {
					errorText [1].text = errorText [3].text = "" + min;
				}
			}
			Lock ();
			if (!played) {
				sound.Play ();
				played = true;
			}
		} else {
			//foreach (Text t in errorText) {
				if (cTime < flickerTime) {
					if (flick) {
						if (!minuteSolved) {
							errorText [0].text = errorText [2].text = Random.Range (1, 13) + ":";

						} else {
							errorText [0].text = errorText[2].text = hour+" :";
							errorText [0].color = Color.cyan;
							errorText [2].color = Color.cyan;
						}
						errorText[1].text = errorText[3].text =	""+Random.Range(0,6)+Random.Range(0,10);
					} else {
						if (!minuteSolved) {
							errorText [0].text = errorText [2].text = Random.Range (1, 13) + ":";
						} else {
							errorText [0].text = errorText[2].text = hour+" :";
							errorText [0].color = Color.cyan;
							errorText [2].color = Color.cyan;
						}
						errorText[1].text = errorText[3].text =	""+Random.Range(0,6)+Random.Range(0,10);
					}

				} else {
					flick = !flick;
					cTime = 0;

				}
			//}
			cTime += Time.deltaTime;

		}
	}

	public bool Output(){
		return solved;
	}

	public Transform GetTransform(){

		return transform;

	}

	public void Lock(){
		solved = true;
	}
	public void Solve(){
		solved = true;
	}
}
