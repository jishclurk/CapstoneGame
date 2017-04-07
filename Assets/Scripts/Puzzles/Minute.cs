using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minute : MonoBehaviour {

	private AudioSource sound;
	public bool pressed;
	private bool disabled;
	TeamManager tm;
	ParticleSystem inactivePS;
	ParticleSystem activePS;
	MinuteScript parentMinute;
	int num;

	void Start () {
		tm = FindObjectOfType<TeamManager> ();
		sound = GetComponent<AudioSource>();
		pressed = false;
		inactivePS = this.transform.FindChild ("ModularPortal").GetComponent<ParticleSystem> ();
		activePS = this.transform.FindChild ("BluePortal").GetComponent<ParticleSystem> ();
		activePS.Stop ();
		inactivePS.Play ();
		parentMinute = transform.parent.gameObject.GetComponent<MinuteScript>();
		disabled = false;

	}

	void OnEnable(){
		inactivePS = this.transform.FindChild ("ModularPortal").GetComponent<ParticleSystem> ();
		activePS = this.transform.FindChild ("BluePortal").GetComponent<ParticleSystem> ();
		activePS.Stop ();
		inactivePS.Play ();
		pressed = false;
	}
	// Update is called once per frame
	void Update () {

	}
	void OnTriggerEnter(Collider other)
	{
		if (!disabled) {
			if (!other.isTrigger && (other.gameObject == tm.activePlayer.gameObject)) {
				sound.Play ();
				Debug.Log ("Button Pressed");
				if (activePS.isPlaying) {
					activePS.Stop ();
					inactivePS.Play ();
					if (parentMinute.activeButton () == num) {
						parentMinute.activePressed(false);
					}
					parentMinute.decrementActive();
				} else if (activePS.isStopped) {
					inactivePS.Stop ();
					activePS.Play ();
					if (parentMinute.activeButton () == num) {
						parentMinute.activePressed(true);
					}

					print ("Time " + num + " pressed");
					parentMinute.incrementActive();
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
