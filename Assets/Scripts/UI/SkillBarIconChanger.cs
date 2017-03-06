using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBarIconChanger : MonoBehaviour {

    [Range(0,3)]
    public int skillIndex;
    public Image skillImage;
    public Text skillText;

    private SwapImageOnButtonPress overlay;

    private TeamManager tm;

	// Use this for initialization
	void Awake () {
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
        overlay = GetComponentInChildren<SwapImageOnButtonPress>();
    }
	
	// Update is called once per frame
	void Update () {
        skillImage.sprite = tm.activePlayer.abilities.abilityArray[skillIndex].image.sprite;
        //skillText.text = tm.activePlayer.abilities.???;
    }
}
