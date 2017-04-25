using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour {

	private TeamManager tm;
	private Image expSlider;

    private float previousHealth;

	// Use this for initialization
	void Start () {
		tm = GameObject.FindWithTag ("TeamManager").GetComponent<TeamManager> ();
		expSlider = GetComponent<Image>();

    }
	
	// Update is called once per frame
	void Update () {
        expSlider.fillAmount = tm.activePlayer.attributes.Experience / (float) tm.activePlayer.attributes.experienceNeededForNextLevel;
    }
}
