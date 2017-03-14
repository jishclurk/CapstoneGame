using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayWithPlayer : MonoBehaviour {

    public Transform player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = player.position;
	}
}
