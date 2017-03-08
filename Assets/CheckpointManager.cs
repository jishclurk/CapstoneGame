using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {

    public List<CheckPoint> checkpoints;

    private SimpleGameManager gm;

    public int completed; //

    public bool levelCompleted;

	// Use this for initialization
	void Awake () {
        checkpoints = new List<CheckPoint>();
        gm = SimpleGameManager.Instance;
        completed = 0;
        levelCompleted = false;
        foreach (Transform child in transform)
        {
            checkpoints.Add(child.gameObject.GetComponent<CheckPoint>());
        }
        Debug.Log("cpmanager got checkpoints");
        Debug.Log("checkpoints=" + checkpoints.Count);
    }

    public void UpdateCheckpoints()
    {
        Debug.Log("Completed a checkpoint");
        completed++;
        if(completed == checkpoints.Count)
        {
            levelCompleted = true;
           // gm.nextLevel();
            Debug.Log("checkpoints complete");
        }
    }

    public void setState(int savedCompleted)
    {
        Debug.Log(savedCompleted);
        Debug.Log(completed);
        foreach(CheckPoint cp in checkpoints)
        {
            Debug.Log(cp);
        }
        Debug.Log("moving players to checkpoint " + savedCompleted.ToString());
        this.completed = savedCompleted;
        GameObject.Find("TeamManager").GetComponent<TeamManager>().setPlayers(checkpoints[savedCompleted-1]);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
