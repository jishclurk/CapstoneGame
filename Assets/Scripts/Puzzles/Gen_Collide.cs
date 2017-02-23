using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gen_Collide : MonoBehaviour {

	public bool triggered;
	private AudioSource sound;
	// Use this for initialization
	void Start () {
		triggered = false;
		sound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if ((other.gameObject.tag == "Player") && (!other.isTrigger)) {
				sound.Play();
				Debug.Log ("Generator flipped");
				triggered = !triggered;
		}
	}
		
}
