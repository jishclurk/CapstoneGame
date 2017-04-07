using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityAlert : MonoBehaviour {

    private KeyCode[] abilityKeys = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R };
    private AudioSource sound;
    private TeamManager tm;
    private Text text;

    // Use this for initialization
    void Awake () {
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
        sound = GetComponent<AudioSource>();
        text = GetComponent<Text>();
        text.CrossFadeAlpha(0, 0, false);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        for (int i = 0; i < abilityKeys.Length; i++)
        {
            if (Input.GetKeyDown(abilityKeys[i]))
            {
                if (!tm.activePlayer.abilities.abilityArray[i].isReady())
                {
                    alertNotReady();
                    return;
                }
                else if (tm.activePlayer.abilities.abilityArray[i].energyRequired > tm.activePlayer.resources.currentEnergy)
                {
                    alertNotEnoughEnergy();
                    return;
                }
            }
        }
    }

    void alertNotReady()
    {
        text.text = "Ability Not Ready Yet";
        text.CrossFadeAlpha(1, 0, false);
        text.CrossFadeAlpha(0, 2, false);
        sound.Play();
    }

    void alertNotEnoughEnergy()
    {
        text.text = "Not Enough Energy";
        text.CrossFadeAlpha(1, 0, false);
        text.CrossFadeAlpha(0, 2, false);
        sound.Play();
    }
}
