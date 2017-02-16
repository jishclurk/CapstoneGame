using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	private TeamManager tm;
	private Slider healthSlider;

	// Use this for initialization
	void Awake () {
		tm = GameObject.FindWithTag ("TeamManager").GetComponent<TeamManager> ();
		healthSlider = GetComponent<Slider> ();
	}
	
	// Update is called once per frame
	void Update () {
		healthSlider.value = tm.playerResources.currentHealth;
	}
}
