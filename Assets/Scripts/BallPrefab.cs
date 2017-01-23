using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPrefab : MonoBehaviour {

	AudioSource[] audio;
	// Use this for initialization
	void Start () {
		audio = gameObject.GetComponents<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision){
		string name = collision.gameObject.name;
		if (name.Contains ("Goal")) {
			Destroy (this.gameObject);
		} else if (name.Contains ("Paddle")) {
			audio [0].Play ();
		}else if (name.Contains ("Wall")) {
			audio [1].Play ();
		}


	}
}
