using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CapstoneGame;

public class EnemyGoal : MonoBehaviour {

	AudioSource audio;
	public GameManager gm;
	public ParticleSystem ballExplode;
	// Use this for initialization
	void Start () {
		audio = gameObject.GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {




	}

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.name.Contains("Ball")){
			audio.Play ();
			Instantiate (ballExplode, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
			gm.PlayerScore++;
		}
	}
		
}