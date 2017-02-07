﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetCamera : MonoBehaviour
{

    public GameObject followPlayer;
    private TeamManager tm;
    private Vector3 offset;

    //Simple script from Roll a Ball
    void Start()
    {
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
        followPlayer = tm.activePlayer;
        offset = transform.position - followPlayer.transform.position;
    }

    void LateUpdate()
    {
        transform.position = followPlayer.transform.position + offset;
    }

}