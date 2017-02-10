using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalManager : MonoBehaviour {

	public List<IGoal> goals;

	public string objectiveName;

	private bool active = false;

	[HideInInspector]
	public TeamManager tm;

	void Awake() {
		goals = new List<IGoal>(GetComponentsInChildren<IGoal> ());
		foreach (IGoal goal in goals) {
			goal.TeamM = this.tm;
		}
	}

	void Update() {
		if (goals.Count == 0) {
			active = true;

		}
	}

	public string goalList(){
		string goalList = "\nGoals:";
			for (int i = 0; i < goals.Count; i++) {
				goalList += "\n" + goals [i].GoalText;
				if (goals [i].IsAchieved ()) {
					goals [i].Complete ();
					goals [i].DestroyGoal ();
					goals.RemoveAt (i);
				}
			}
		if (goals.Count == 0) {
			goalList = "";
		}
		return goalList;
	}

    public void setActive(bool active)
    {
        this.active = active;
    }

	public bool isComplete(){
		return active;
	}

}
	

