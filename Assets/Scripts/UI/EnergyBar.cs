using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour {

	private TeamManager tm;
	private Slider energySlider;

	// Use this for initialization
	void Awake () {
		tm = GameObject.FindWithTag ("TeamManager").GetComponent<TeamManager> ();
		energySlider = GetComponent<Slider> ();
	}

	// Update is called once per frame
	void Update () {
		energySlider.value = tm.playerResources.currentEnergy;
	}
}
