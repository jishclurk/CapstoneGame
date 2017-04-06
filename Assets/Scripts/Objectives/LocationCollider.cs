using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationCollider : MonoBehaviour {

	bool proximityTrigger;
	TeamManager tm;
	// Use this for initialization
	void Start () {
		proximityTrigger = false;
		tm = FindObjectOfType<TeamManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if ((other.gameObject == tm.activePlayer.gameObject) && (!other.isTrigger)) {
			proximityTrigger = true;

		} 
		//		else {
		//			popUpObject.gameObject.SetActive (false);
		//			active = false;
		//		}
	}

	public bool EnteredLocation(){

		return proximityTrigger;
	}
}
