using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupText : MonoBehaviour {

	// Use this for initialization
	Camera cam;
	void Start () {
		cam = GetComponent<Canvas> ().rootCanvas.worldCamera;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 v = cam.transform.position - transform.position;
		v.x = v.z = 0.0f;
		transform.LookAt( cam.transform.position - v ); 
		transform.Rotate(0,180,0);
	}
}
