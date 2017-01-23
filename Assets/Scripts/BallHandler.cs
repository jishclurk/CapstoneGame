using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame{


	public class BallHandler : MonoBehaviour {


		public GameObject ballPrefab;

		public List<Ball> balls;

		[Range(1,4)]
		public int numBalls = 1;


		[Range(1.0010f,1.0100f)]
		public float speedUp = 1.0010f;

		// Use this for initialization
		void Awake () {
			balls = new List<Ball> ();
			for (int i = 0; i < numBalls; i++) {
				balls.Add (new Ball (Instantiate (ballPrefab, new Vector3 (0, 0+2*i, 0), transform.rotation)));
			}

		}

		void Start(){
			foreach (Ball ball in balls) {
				ball.SetSpeedUpConstant (speedUp);
				ball.ballObj.GetComponent<Rigidbody> ().velocity = new Vector3 (5, 3, 0);
			}

		}
		
		// Update is called once per frame
		void FixedUpdate () {

			List<Ball> deleteList = new List<Ball> ();

			int deleted = 0;
			foreach (Ball b in balls){
				if (b.ballObj != null) {
					b.UpdatePhysics (new Vector3 (0, 0, 0));
				} else {
					deleted++;

				}
			}

			foreach (Ball d in deleteList) {
				balls.Remove (d);
			}

				
		}

		public Ball GetUnWatchedBall(){
			for (int i=0; i<numBalls; i++){
				if (!balls [i].BeingWatched ()) {
					balls [i].SetWatched (true);
					return balls [i];
				}
			}
			return null;
		}

	//	void OnGUI(){
	//		GUI.TextArea(new Rect (0,0,100,50),"Enemy Score   "+eScore);
	//		GUI.TextArea(new Rect (Screen.width-100,0,100,50),"PLayer Score   "+pScore);
	//
	//		if (pGoal && !endCondition) {
	//			if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height / 2 + 100, 200, 100), "Start New Round")) {
	//				rb.velocity = new Vector3 (5, 3, 0);\
	//				pGoal = false;
	//			}
	//		}
	//
	//		if (eGoal && !endCondition) {
	//			if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height / 2 + 100, 200, 100), "Start New Round")) {
	//				rb.velocity = new Vector3 (5, 3, 0);
	//				eGoal = false;
	//			}
	//		}
	//
	//		if (endCondition) {
	//			if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height / 2 + 100, 200, 100), "Quit?")) {
	//				Application.Quit ();
	//			}
	//			if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "Game Over, New game?")) {
	//				SceneManager.LoadScene("Pong");
	//			}
	//		}
	//	}
		}
}
