using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Add this to GoalManager to run a "kill the enemies" goal:    
public class ReachLocation : MonoBehaviour ,IGoal{

	public LocationCollider location;



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
	}



	public bool IsAchieved() {
		return (location.EnteredLocation());
		//return false;
	}

	public void Complete() {
		//ScoreSingleton.score += 50;
		//audio.Play(trumpetSound);
		//checkpoint.openCheckPointScreen();
	}

	public void DestroyGoal() {
		Destroy (this.gameObject);
	}
}

