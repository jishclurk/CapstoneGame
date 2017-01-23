using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGoal : MonoBehaviour {

	private int pScore;
	AudioSource audio;

	public ParticleSystem ballExplode;
	// Use this for initialization
	void Start () {
		pScore = 0;
		audio = gameObject.GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {




	}

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.name.Contains("Ball")){
			audio.Play ();
			Instantiate (ballExplode, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
			pScore++;
		}
	}

	void OnGUI(){
		GUI.TextArea(new Rect (Screen.width-100,0,100,50),"Player Score   "+pScore);
	}
}