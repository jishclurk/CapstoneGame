using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartDirection : MonoBehaviour {

	private Rigidbody rb;

	int eScore = 0;
	int pScore =0;
	bool eGoal = false;
	bool pGoal = false;
	bool endCondition = false;

	[Range(1.0010f,1.0100f)]
	public float speedUp = 1.0010f;

	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody> ();
		rb.velocity = new Vector3 (5, 3, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		rb.velocity = rb.velocity * speedUp;

		if (rb.velocity.x < 2f && rb.velocity.x > -2.0f) {
			float newX = rb.velocity.x * 1.5f;
			rb.velocity = new Vector3 (newX, rb.velocity.y, rb.velocity.z);
		}

		if (rb.velocity.y < 2.0f && rb.velocity.y > -2.0f) {
			float newY = rb.velocity.y * 1.5f;
			rb.velocity = new Vector3 (rb.velocity.x, newY, rb.velocity.z);
		}

		if (transform.position.x < -16) {
			transform.position = new Vector3 (0, 0, 0);
			rb.velocity = new Vector3 (0, 0, 0);
			//player score
			pGoal = true;
			pScore++;
		}

		if (transform.position.x > 16) {
			transform.position = new Vector3 (0, 0, 0);
			rb.velocity = new Vector3 (0, 0, 0);
			//enemy score
			eGoal = true;
			eScore++;
		}

		endCondition = (pScore == 3 || eScore == 3);
			
	}

	void OnGUI(){
		GUI.TextArea(new Rect (0,0,100,50),"Enemy Score   "+eScore);
		GUI.TextArea(new Rect (Screen.width-100,0,100,50),"PLayer Score   "+pScore);

		if (pGoal && !endCondition) {
			if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height / 2 + 100, 200, 100), "Start New Round")) {
				rb.velocity = new Vector3 (5, 3, 0);
				pGoal = false;
			}
		}

		if (eGoal && !endCondition) {
			if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height / 2 + 100, 200, 100), "Start New Round")) {
				rb.velocity = new Vector3 (5, 3, 0);
				eGoal = false;
			}
		}

		if (endCondition) {
			if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height / 2 + 100, 200, 100), "Quit?")) {
				Application.Quit ();
			}
			if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "Game Over, New game?")) {
				SceneManager.LoadScene("Pong");
			}
		}
	}
}
