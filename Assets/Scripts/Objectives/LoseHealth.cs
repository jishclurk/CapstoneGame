using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Add this to GoalManager to run a "kill the enemies" goal:    
public class LoseHealth : MonoBehaviour ,IGoal{

	public int healthLost= 50;
	public AudioClip trumpetSound;

	//PlayerHealth playerHealth;
	public string goalText;
	public string GoalText { 
		get {
			return goalText;

		}
		set {
			value = goalText;
		}
	}

	void Awake() {
		//playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
	}

	public bool IsAchieved() {
		//return (playerHealth.currentHealth <= healthLost);
		return false;
	}

	public void Complete() {
		//ScoreSingleton.score += 50;
		//audio.Play(trumpetSound);
	}

	public void DestroyGoal() {
		Destroy (this.gameObject);
	}
}

