using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockPuzzle : MonoBehaviour {

	bool btest;
	int hour;
	int min;
	Light genSpot;
	bool turnedOn;
	HourScript hourPart;
	MinuteScript minutePart;
	ClockCollide trigger;

	bool complete;
	// Use this for initialization
	void Start () {
		hour = System.DateTime.Now.Hour;
		min = System.DateTime.Now.Minute;
		btest = true;
		genSpot = this.gameObject.transform.Find ("Spot Light").GetComponent<Light> ();
		turnedOn = false;
		hourPart = this.transform.FindChild ("Hours").GetComponent<HourScript> ();
		minutePart = this.transform.FindChild ("Minutes").GetComponent<MinuteScript> ();
		trigger = this.gameObject.transform.Find ("ColliderEq_6").GetComponent<ClockCollide> ();
		complete = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (trigger.triggered && !complete) {
			hourPart.gameObject.SetActive (true);
			minutePart.gameObject.SetActive (true);
			hour = System.DateTime.Now.Hour % 12;
			min = System.DateTime.Now.Minute;
			if (hour == 0) {
				hour = 12;
			}
			print ("\nHour: " + hour + "\nMinute: " + min);
			hourPart.setActiveButton (hour);
			minutePart.setActiveButton (min);
			if (hourPart.Complete () && minutePart.Complete ()) {
				print ("Clock Accomplished");
				complete = true;
				hourPart.Disable ();
				minutePart.Disable ();
				genSpot.intensity = 100;

			} else {
				genSpot.intensity = 0;
			}
		} else if (complete) {

		} else {
			hourPart.ResetTime ();
			minutePart.ResetTime ();
			hourPart.gameObject.SetActive (false);
			minutePart.gameObject.SetActive (false);
		}
	}
		

	public bool Output(){
		return hourPart.Complete () && minutePart.Complete ();
	}
}
