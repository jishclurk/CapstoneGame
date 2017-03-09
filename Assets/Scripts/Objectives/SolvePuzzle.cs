using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Add this to GoalManager to run a "kill the enemies" goal:    
public class SolvePuzzle: MonoBehaviour ,IGoal{

	public AudioClip trumpetSound;
	public CircuitPuzzle cp;


	//PlayerHealth playerHealth;
	public string goalText;
	private TeamManager tm;

	public TeamManager TeamM { 
		get {
			return tm;

		}
		set {
			tm = value;
		}
	}

	public string GoalText { 
		get {
			return goalText;

		}
		set {
			goalText = value;
		}
	}

	void Awake() {
		//playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
		//checkpoint = GameObject.Find("CheckPointTrigger").GetComponent<CheckPoint>();
	}

	public bool IsAchieved() {
		return (cp.Output());
		//return false;
	}

	public void Complete() {
		//cp.Sol
		cp.Solve();
		cp.Lock ();

		//ScoreSingleton.score += 50;
		//audio.Play(trumpetSound);
		//checkpoint.openCheckPointScreen();
	}

	public void DestroyGoal() {
		Destroy (this.gameObject);
	}
}

