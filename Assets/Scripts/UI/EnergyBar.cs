using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour {

    [Range(1, 4)]
    public int ID;

    private TeamManager tm;
    private Image energySlider;
    private PlayerResources resources;

    // Use this for initialization
    void Start()
    {
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
        energySlider = GetComponent<Image>();
        resources = tm.playerList[ID - 1].resources;
    }

    // Update is called once per frame
    void Update()
    {
        energySlider.fillAmount = resources.currentEnergy / resources.maxEnergy;
    }
}
