﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour, ICircuitPiece {


	public Generator activator;
	bool turnedOn;
	bool complete;
	Gen_Collide trigger;
	Light genSpot;
	ParticleSystem ps;
	// Use this for initialization
	void Start () {
		genSpot = this.gameObject.transform.FindChild ("Spot Light").GetComponent<Light> ();
		turnedOn = false;
		complete = false;
		trigger = this.gameObject.transform.FindChild ("ColliderEq_5").GetComponent<Gen_Collide> ();
		ps = this.transform.FindChild ("T2BlueLootItem").GetComponent<ParticleSystem> ();
		ps.Stop ();
	}
	
	// Update is called once per frame
	void Update () {

		turnedOn = trigger.triggered;

		if (turnedOn) {
			if (activator.Output ()) {
				if (ps.isStopped ) {
					ps.Play ();
				}
				complete = true;
			} else {
				if (ps.isPlaying) {
					ps.Stop ();
				}
				complete = false;
			}
			genSpot.intensity = 100;
		} else {
			genSpot.intensity = 0;
		}


	}

	public bool Output(){

		return complete;
	}

	public Transform GetTransform(){
		return transform;

	}
}