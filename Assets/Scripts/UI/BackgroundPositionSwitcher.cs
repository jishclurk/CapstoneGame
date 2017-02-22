using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPositionSwitcher : MonoBehaviour {

    public float[] positions = new float[4];

    private TeamManager tm;
    private float epsilon = 0.1f;

	// Use this for initialization
	void Awake () {
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
        
    }
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = new Vector3(transform.localPosition.x, positions[tm.activePlayer.id - 1], transform.localPosition.z);
	}
}
