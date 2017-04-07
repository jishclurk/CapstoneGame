using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Add this to GoalManager to run a "kill the enemies" goal:    
public class TurnOnGenerator : MonoBehaviour ,IGoal{

	private CheckPoint checkpoint;
	public AudioClip trumpetSound;
	public Generator gen;


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
		return (gen.isSending());
		//return false;
	}

	public void Complete() {
		//ScoreSingleton.score += 50;
		//audio.Play(trumpetSound);
		//checkpoint.openCheckPointScreen();
		gen.Solve();
	}

	public void DestroyGoal() {
		Destroy (this.gameObject);
	}
}

