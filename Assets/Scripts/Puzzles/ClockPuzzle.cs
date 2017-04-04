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
	// Use this for initialization
	void Start () {
		hour = System.DateTime.Now.Hour;
		min = System.DateTime.Now.Minute;
		btest = true;
		genSpot = this.gameObject.transform.Find ("Spot Light").GetComponent<Light> ();
		turnedOn = false;
		hourPart = this.transform.FindChild ("Hours").GetComponent<HourScript> ();
		minutePart = this.transform.FindChild ("Minutes").GetComponent<MinuteScript> ();

	}
	
	// Update is called once per frame
	void Update () {

		hour = System.DateTime.Now.Hour % 12;
		min = System.DateTime.Now.Minute;
		if (hour == 0) {
			hour = 12;
		}
		if (btest) {
			print ("\nHour: " + hour+"\nMinute: "+min);
			btest = false;
		}
		hourPart.setActiveButton(hour);
		minutePart.setActiveButton (min);
		if (hourPart.Complete() && minutePart.Complete()) {
			print ("Clock Accomplished");
			genSpot.intensity = 100;

		} else {
			genSpot.intensity = 0;
		}
	}

	public bool hourComplete(){
		return hourPart.Complete ();
	}

	public bool Output(){
		return hourPart.Complete () && minutePart.Complete ();
	}
}
