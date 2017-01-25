using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    // Use this for initialization
    private float exitTime = 3.5f;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.DownArrow)) {
			transform.Translate (0.0f, -0.25f, 0.0f);
		}
		if (Input.GetKey (KeyCode.UpArrow)) {
			transform.Translate (0.0f, 0.25f, 0.0f);
		}

		if (transform.position.y > 7.5f) {
			transform.position = new Vector3 (15.0f, 7.5f, 0.0f);
		}
		if (transform.position.y < -7.5f) {
			transform.position = new Vector3 (15.0f, -7.5f, 0.0f);
		}
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            //Switch to exit state scene before running below
            while (exitTime > 0)
            {
                exitTime -= Time.deltaTime;
            }
                Application.Quit();
            
        }


	}
}
