using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownTimerScript : MonoBehaviour {

    public Image Overlay;
    public Text Timer;
    public int index = 0;
    private TeamManager tm;

    void Awake ()
    {
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        ISpecial abil = tm.activePlayer.abilities.abilityArray[index];
        if (abil != null)
        {
            bool visible = !(abil.isReady());
            Overlay.gameObject.SetActive(visible);
            Timer.gameObject.SetActive(visible);
            if (visible)
            {
                float timeLeft = abil.RemainingTime();
                float totalTime = abil.coolDownTime;
                Overlay.fillAmount = timeLeft / totalTime;
                Timer.text = ((int) Mathf.Ceil(timeLeft)).ToString();
            }
        }
	}
}
