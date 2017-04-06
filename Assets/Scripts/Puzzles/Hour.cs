using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hour : MonoBehaviour {

	private AudioSource sound;
	public bool pressed;
	private bool disabled;
	TeamManager tm;
	ParticleSystem inactivePS;
	ParticleSystem activePS;
	HourScript parentHour;


	int num;

	void Start () {
		tm = FindObjectOfType<TeamManager> ();
		sound = GetComponent<AudioSource>();
		pressed = false;
		inactivePS = this.transform.FindChild ("ModularPortal").GetComponent<ParticleSystem> ();
		activePS = this.transform.FindChild ("YellowPortal").GetComponent<ParticleSystem> ();
		activePS.Stop ();
		inactivePS.Play ();
		parentHour = (HourScript)transform.parent.gameObject.GetComponent<HourScript>();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnEnable(){
//		tm = FindObjectOfType<TeamManager> ();
//		sound = GetComponent<AudioSource>();
//		pressed = false;
//		inactivePS = this.transform.FindChild ("ModularPortal").GetComponent<ParticleSystem> ();
//		activePS = this.transform.FindChild ("YellowPortal").GetComponent<ParticleSystem> ();
		inactivePS = this.transform.FindChild ("ModularPortal").GetComponent<ParticleSystem> ();
		activePS = this.transform.FindChild ("YellowPortal").GetComponent<ParticleSystem> ();
		activePS.Stop ();
		inactivePS.Play ();
		pressed = false;
//
	}
	void OnTriggerEnter(Collider other)
	{
		if (!disabled) {
			if (!other.isTrigger && (other.gameObject == tm.activePlayer.gameObject)) {
				sound.Play ();
				//Debug.Log ("Button Pressed");
				pressed = !pressed;

				if (activePS.isPlaying) {
					activePS.Stop ();
					inactivePS.Play ();
					if (parentHour.activeButton () == num) {
						parentHour.activePressed(false);
					}
					parentHour.decrementActive();
				} else if (activePS.isStopped) {
					inactivePS.Stop ();
					activePS.Play ();
					if (parentHour.activeButton () == num) {
						parentHour.activePressed(true);
					}

					print ("Time " + num + " pressed");
					parentHour.incrementActive();
				}


				//GameObject.FindGameObjectWithTag("Door").SetActive(false);
			}
		}
	}

	public void setTime(int i ){
		num = i;
	}
		



	public bool Pressed(){
		return pressed;
	}

	public void Disable(){
		disabled = true;
	}
}
