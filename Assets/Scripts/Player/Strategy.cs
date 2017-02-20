using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strategy : MonoBehaviour {

    public bool isplayerControlled; //who starts as AI controlled or not will be set in inspector for now.

    private PlayerController playerScript;
    private CoopAiController aiScript;

	// Use this for initialization
	void Start () {
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
        playerScript.ResetOnSwitch(); //reset player to default values Why do this? reset run and targets
        //set values based on previous ai control
        if (aiScript.targetedEnemy != null)
        {
            playerScript.enemyClicked = true;
            playerScript.targetedEnemy = aiScript.targetedEnemy;
        }
       
        aiScript.enabled = false;
        playerScript.enabled = true;

    }

    public void setAsCoopAI()
    {
        isplayerControlled = false;
        playerScript.ResetOnSwitch(); //reset player to default values. Why do this? Disable AOETarget
        aiScript.ResetOnSwitch(); //reset ai default values
        //set values based on previous player control


        aiScript.enabled = true;
        playerScript.enabled = false;

    }

    public void setAsDead()
    {
        isplayerControlled = false;
        aiScript.enabled = false;
        playerScript.enabled = false;
    }
}
