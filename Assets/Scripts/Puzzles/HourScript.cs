using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HourScript : MonoBehaviour {

	public Hour[] hourButtons;
	public Hour hourButton;
	int r =5;
	int incpressed;
	int activeNum;
	bool pressed;

	// Use this for initialization
	void Start () {
		hourButtons = new Hour[12];
		for (int i =0; i<12;i++){
			hourButtons[i] = (Hour) Instantiate (hourButton, new Vector3 (transform.parent.transform.position.x + 
				r*Mathf.Cos(-Mathf.PI*2*(i+1)/12), transform.parent.transform.position.y, 
				transform.parent.transform.position.z+ r*Mathf.Sin(-Mathf.PI*2*(i+1)/12)), Quaternion.identity);
			hourButtons[i].transform.parent = gameObject.transform;
			hourButtons [i].setTime (i + 1);
		}
	}
	
	// Update is called once per frame
	void Update () {

		//if ( 


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
		foreach (Hour h in hourButtons) {
			h.Disable ();
		}

	}

	public bool Complete(){
		return (pressed && incpressed == 1);
	}

}
