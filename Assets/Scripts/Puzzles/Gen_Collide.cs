using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gen_Collide : MonoBehaviour {

	public bool triggered;
	private AudioSource sound;
	GameObject popUpObject;
	TeamManager tm;
	bool active;
	// Use this for initialization
	void Start () {
		triggered = false;
		active = false;
		sound = GetComponent<AudioSource>();
		tm = FindObjectOfType<TeamManager> ();
		popUpObject = transform.FindChild ("PopupText").gameObject;
		popUpObject.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (active) {
			if (Input.GetKeyDown(KeyCode.G)){
				sound.Play ();
				//Debug.Log ("Generator flipped");
				triggered = !triggered;
			}

		}
	}

	void OnTriggerEnter(Collider other){
		if ((other.gameObject == tm.activePlayer.gameObject) && (!other.isTrigger)) {
			popUpObject.gameObject.SetActive (true);
			active = true;

		} 
//		else {
//			popUpObject.gameObject.SetActive (false);
//			active = false;
//		}
	}

	void OnTriggerExit(Collider other){
		if ((other.gameObject == tm.activePlayer.gameObject) && (!other.isTrigger)) {
			popUpObject.gameObject.SetActive (false);
					active = false;
		}
	}
		
}
