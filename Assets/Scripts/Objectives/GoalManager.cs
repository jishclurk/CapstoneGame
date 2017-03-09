using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalManager : MonoBehaviour {

	public List<IGoal> goals;

	public string objectiveName;

	private bool active = false;

	private bool loadCompleted;

	string goalListString;

	int achievedGoals;

	[HideInInspector]
	public TeamManager tm;

	void Awake() {
		goals = new List<IGoal>(GetComponentsInChildren<IGoal> ());
		foreach (IGoal goal in goals) {
			goal.TeamM = this.tm;
		}
		achievedGoals = 0;
		loadCompleted = false;
	}

	void Update() {
		goalListString = "\nGoals:";
		achievedGoals = 0;
		if (loadCompleted) {
			achievedGoals = goals.Count;
			for (int i = 0; i < goals.Count; i++) {
				goals [i].Complete ();
			}

		} else {
			for (int i = 0; i < goals.Count; i++) {
				//goalList += "\n" + goals [i].GoalText;
				//if (goals [i].IsAchieved ()) {
				//					goals [i].Complete ();
				//					goals [i].DestroyGoal ();
				//					goals.RemoveAt (i);
				//}
				if (!goals [i].IsAchieved ()) {
					goalListString += "\n" + goals [i].GoalText;
				} else {
					goals [i].Complete ();
					achievedGoals++;
				}
			}
		}
		if (goals.Count == achievedGoals) {
			goalListString = "";
			active = true;

		} else {
			active = false;
		}
	}

	public string goalList(){

		return this.goalListString;
	}

	public void CompleteGoals(bool complete){
		loadCompleted = complete;


	}

    public void setActive(bool active)
    {
        this.active = active;
 
    }

	public bool isComplete(){
		return active;
	}

}
	

