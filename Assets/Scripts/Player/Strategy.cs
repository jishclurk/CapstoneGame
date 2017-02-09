using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strategy : MonoBehaviour {

    public bool isplayerControlled; //who starts as AI controlled or not will be set in inspector for now.

    private PlayerController playerScript;
    private CoopAiController aiScript;

	// Use this for initialization
	void Awake () {
        playerScript = GetComponent<PlayerController>();
        aiScript = GetComponent<CoopAiController>();
        if (isplayerControlled)
        {
            setAsPlayer();
        }
        else
        {
            setAsCoopAI();
        }
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setAsPlayer()
    {
        isplayerControlled = true;
        aiScript.enabled = false;
        playerScript.enabled = true;

    }

    public void setAsCoopAI()
    {
        isplayerControlled = false;
        aiScript.enabled = true;
        playerScript.enabled = false;

    }
}
