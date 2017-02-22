using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    [Range(1, 4)]
    public int ID;

	private TeamManager tm;
	private Image healthSlider;
    private PlayerResources resources;

	// Use this for initialization
	void Start() {
		tm = GameObject.FindWithTag ("TeamManager").GetComponent<TeamManager> ();
		healthSlider = GetComponent<Image>();
        resources = tm.playerList[ID - 1].resources;
    }
	
	// Update is called once per frame
	void Update () {
		healthSlider.fillAmount = resources.currentHealth / resources.maxHealth;
	}
}
