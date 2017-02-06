using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour {

	public List<GoalManager> objectives;

	public Text goalListText;
	private bool activeObjective;
	private string objectiveList;
	private int activeObjNum = 0;

	void Awake() {
		objectives = new List<GoalManager>(GetComponentsInChildren<GoalManager> ());
		activeObjective = false;
		objectiveList =  "Objectives: \n";
	}

	void Update() {
		objectiveList =  "Objectives: \n";
		for (int i = 0; i<objectives.Count ; i++){
			objectiveList += "\n"+(i+1)+".  " + objectives[i].objectiveName;
			if (!objectives [i].isComplete () && !activeObjective) {
				activeObjective = true;
				activeObjNum = i;
			} 
			if (!objectives [i].isComplete () && activeObjNum == i) {
				objectiveList += "\n" + objectives [i].goalList () +"\n";
			}
			if (objectives [i].isComplete () && activeObjective) {
				activeObjective = false;
			}
		}
		goalListText.text = objectiveList;
	}
}


