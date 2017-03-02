﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour {

    private GameObject followPlayer;
    private TeamManager tm;
    private Vector3 offset;

    //Simple script from Roll a Ball
    void Start()
    {
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
        if (tm)
        {
            followPlayer = tm.activePlayer.gameObject;
            if (followPlayer)
            {
                offset = transform.position - followPlayer.transform.position;
                // Debug.Log("offset:" + offset);
            }
            else
            {
                Debug.Log("ERROR: Minimap Camera cannot locate active player character!");
            }
        }
        else {
            Debug.Log("ERROR: Minimap Camera cannot locate Team Manager!");
        }
        
    }

    void LateUpdate()
    {
        if (transform.position != followPlayer.transform.position + offset)
        {
            transform.position = Vector3.Lerp(transform.position, followPlayer.transform.position + offset, 0.2f);
        }
        else
        {
            transform.position = followPlayer.transform.position + offset;
        }
    }

}