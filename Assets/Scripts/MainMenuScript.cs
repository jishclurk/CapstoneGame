using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

	bool increasingRot = false;

	//Subtle camera rotation
	void Update () {
		if (increasingRot) {
			transform.Rotate (new Vector3 (0, 0.05f, 0));
			if (transform.eulerAngles.y > 45.0f && transform.eulerAngles.y < 46.0f)
				increasingRot = false;
		} else {
			transform.Rotate (new Vector3 (0, -0.05f, 0));
			if (transform.eulerAngles.y < 330.0f && transform.eulerAngles.y > 329.0f)
				increasingRot = true;
		}
	
	}

}
