using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {

    public List<CheckPoint> checkpoints;

    private SimpleGameManager gm;

    public int completed; //

	// Use this for initialization
	void Start () {
        checkpoints = new List<CheckPoint>();
        gm = SimpleGameManager.Instance;
        completed = 0;
        foreach (Transform child in transform)
        {
            checkpoints.Add(child.gameObject.GetComponent<CheckPoint>());
        }
    }

    public void UpdateCheckpoints()
    {
        completed++;
        if(completed == checkpoints.Count-1)
        {
            gm.nextLevel();
            Debug.Log("checkpoints complete");
        }
    }

    public void setState(int completed)
    {
        this.completed = completed;
        GameObject.Find("TeamManager").GetComponent<TeamManager>().setPlayers(checkpoints[completed]);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
