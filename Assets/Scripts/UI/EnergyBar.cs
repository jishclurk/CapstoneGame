using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour {

    [Range(1,4)]
    public int playerIndex;

	private TeamManager tm;
	private Image energySlider;

	// Use this for initialization
	void Awake () {
		tm = GameObject.FindWithTag ("TeamManager").GetComponent<TeamManager>();
        energySlider = GetComponent<Image>();
	}

	// Update is called once per frame
	void Update () {
		//energySlider.fillAmount = NEED TEAM MANAGER STUFF
	}
}
