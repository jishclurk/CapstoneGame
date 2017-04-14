﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {

    public List<CheckPoint> checkpoints;

    private SimpleGameManager gm;

	// Use this for initialization
	void Awake () {
        checkpoints = new List<CheckPoint>();
        gm = GameObject.Find("GameManager").GetComponent<SimpleGameManager>();
        foreach (Transform child in transform)
        {
            checkpoints.Add(child.gameObject.GetComponent<CheckPoint>());
        }
    }

    public void setState(int checkpointToStartAt)
    {

        GameObject.Find("TeamManager").GetComponent<TeamManager>().setPlayers(checkpoints[checkpointToStartAt]);
    }

    public int GetCheckPoint(CheckPoint child)
    {
        
        for(int i = 0; i <checkpoints.Count; i++)
        {
            if(checkpoints[i] == child)
            {
                return i;
            }
        }
        Debug.Log("GET CHECKPOINT DIDN'T WORK");
        return -1;
    }

    public void UpdateButtons(string name)
    {
        foreach(Transform child in transform)
        {
            GameObject terminal = child.gameObject;
            terminal.GetComponentInChildren<SaveListController>().UpdateButtons(name);
        }
    }
	
}
