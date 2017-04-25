using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemaCam : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey(KeyCode.RightArrow)){
			transform.Translate (new Vector3 (0.01f, 0.0f, 0.0f ));
		}
		if (Input.GetKey(KeyCode.LeftArrow)){
			transform.Translate (new Vector3 (-0.01f, 0.0f, 0.0f ));
		}
		if (Input.GetKey(KeyCode.UpArrow)){
			transform.Translate (new Vector3 (0.00f, 0.01f, 0.0f ));
		}
		if (Input.GetKey(KeyCode.DownArrow)){
			transform.Translate (new Vector3 (0.0f, -0.01f, 0.0f ));
		}
	}
}
