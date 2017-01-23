using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGoal : MonoBehaviour {

	private int eScore;
	AudioSource audio;

	public ParticleSystem ballExplode;
	// Use this for initialization
	void Start () {
		eScore = 0;
		audio = gameObject.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {




	}

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.name.Contains("Ball")){
			audio.Play ();
			Instantiate (ballExplode, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
			eScore++;
		}
	}

	void OnGUI(){
		GUI.TextArea(new Rect (0,0,100,50),"Enemy Score   "+eScore);
	}
}
