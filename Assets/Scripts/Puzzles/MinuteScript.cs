using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinuteScript : MonoBehaviour {

	int r =12;
	public Minute[] minuteButtons;
	public Minute minuteButton;
	int incpressed;
	int activeNum;
	bool pressed;
	// Use this for initialization
	void Start () {
		minuteButtons = new Minute[60];
		for (int i =0; i<60;i++){
			minuteButtons[i] = (Minute) Instantiate (minuteButton, new Vector3 (transform.parent.transform.position.x + 
				r*Mathf.Cos(-Mathf.PI*2*i/60), transform.parent.transform.position.y, 
				transform.parent.transform.position.z+ r*Mathf.Sin(-Mathf.PI*2*i/60)), Quaternion.identity);
			minuteButtons[i].transform.parent = gameObject.transform;
			minuteButtons [i].setTime (i);
			//minuteButton.transform.parent = gameObject.transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int activeButton(){

		return activeNum;

	}

	public void ResetTime(){
		incpressed = 0;
		pressed = false;
	}

	public void setActiveButton(int i){
		activeNum = i;
	}

	public void incrementActive(){
		incpressed++;
	}

	public void decrementActive(){
		incpressed--;
	}

	public void activePressed(bool p){

		pressed = p;
	}

	public void Disable(){
		foreach (Minute m in minuteButtons) {
			m.Disable ();
		}

	}

	public bool Complete(){
		return (pressed && incpressed == 1);
	}
}
