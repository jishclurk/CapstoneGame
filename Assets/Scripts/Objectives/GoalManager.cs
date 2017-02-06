using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalManager : MonoBehaviour {

	public List<IGoal> goals;

	public string objectiveName;

	private bool active = false;

	void Awake() {
		goals = new List<IGoal>(GetComponentsInChildren<IGoal> ());
	}

	void Update() {
		if (goals.Count == 0) {
			active = true;

		}
	}

	public string goalList(){
		string goalList = "\n\t\tGoals:";
			for (int i = 0; i < goals.Count; i++) {
				goalList += "\n\t\t\t - " + goals [i].GoalText;
				if (goals [i].IsAchieved ()) {
					goals [i].Complete ();
					goals [i].DestroyGoal ();
					goals.RemoveAt (i);
				}
			}
		return goalList;
	}

	public bool isComplete(){
		return active;
	}
}
	

