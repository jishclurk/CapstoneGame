using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    [Range(1, 4)]
    public int ID;

    public Image damageIndicator;

	private TeamManager tm;
	private Image healthSlider;
    private PlayerResources resources;
    private float previousHealth;

	// Use this for initialization
	void Start () {
		tm = GameObject.FindWithTag ("TeamManager").GetComponent<TeamManager> ();
		healthSlider = GetComponent<Image>();
        resources = tm.playerList[ID - 1].resources;

        damageIndicator.CrossFadeAlpha(0, 0, true);
        previousHealth = resources.currentHealth;
    }
	
	// Update is called once per frame
	void Update () {
		healthSlider.fillAmount = resources.currentHealth / PlayerResources.maxHealth;

        if (resources.currentHealth < previousHealth)
        {
            damageIndicator.CrossFadeAlpha(1, 0, true);
            damageIndicator.CrossFadeAlpha(0, 1, true);
        }

        previousHealth = resources.currentHealth;
    }
}
