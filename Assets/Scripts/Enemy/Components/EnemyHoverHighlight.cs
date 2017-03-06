using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHoverHighlight : MonoBehaviour {

    public GameObject hoverHighlight;
    public GameObject selectedHighlight;
    
    private TeamManager tm;
    private EnemyHealth health;
    private bool hovering;

	void Awake () {
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
        health = GetComponent<EnemyHealth>();
        hovering = false;

        if (hoverHighlight != null)
            hoverHighlight.gameObject.SetActive(false);
        if (selectedHighlight != null)
            selectedHighlight.gameObject.SetActive(false);
    }
	
    void Update()
    {
        if (selectedHighlight != null && !hovering && tm.activePlayer.strategy.playerScript.targetedEnemy == transform && !health.isDead)
            selectedHighlight.gameObject.SetActive(true);
        else if (selectedHighlight != null)
        {
            selectedHighlight.gameObject.SetActive(false);
        }
    }

	void OnMouseEnter()
    {
        if (hoverHighlight != null)
        {
            hovering = true;
            hoverHighlight.gameObject.SetActive(true);
            if (selectedHighlight != null)
                selectedHighlight.gameObject.SetActive(false);
        }
            
    }

    void OnMouseExit()
    {
        if (hoverHighlight != null)
        {
            hovering = false;
            hoverHighlight.gameObject.SetActive(false);
        }
    }
}
