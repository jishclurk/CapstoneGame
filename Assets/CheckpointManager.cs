using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {

    public List<CheckPoint> checkpoints;

    private SimpleGameManager gm;

    //number of checkpoints reaches this level
    //public int completed; 

    public bool levelCompleted;

	// Use this for initialization
	void Awake () {
        checkpoints = new List<CheckPoint>();
        gm = GameObject.Find("GameManager").GetComponent<SimpleGameManager>();
        //completed = 0;
        levelCompleted = false;
        foreach (Transform child in transform)
        {
            checkpoints.Add(child.gameObject.GetComponent<CheckPoint>());
        }
        Debug.Log("cpmanager got checkpoints");
        Debug.Log("checkpoints=" + checkpoints.Count);
    }

    //public void UpdateCheckpoints()
    //{
    //    Debug.Log("Completed a checkpoint");
    //    completed++;
    //}

    public void setState(int checkpointToStartAt)
    {
        Debug.Log(checkpointToStartAt);
        //Debug.Log(completed);
        foreach(CheckPoint cp in checkpoints)
        {
            Debug.Log(cp);
        }
        Debug.Log("moving players to checkpoint " + checkpointToStartAt.ToString());
        //this.completed = checkpointToStartAt;
        GameObject.Find("TeamManager").GetComponent<TeamManager>().setPlayers(checkpoints[checkpointToStartAt]);
    }

    public void FinallCheckPointReached()
    {
        levelCompleted = true;
        //gm.nextLevel();
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
