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
	private int completedObjectives = 0;
	public TeamManager tm;

	void Awake() {
		objectives = new List<GoalManager>(GetComponentsInChildren<GoalManager> ());
		foreach (GoalManager gm in objectives){
			gm.tm = this.tm;

		}
		activeObjective = false;
		objectiveList =  "Objectives: \n";
	}

	void Update() {
		objectiveList =  "Objectives: \n";
		for (int i = 0; i<objectives.Count ; i++){
			objectiveList += "\n"+(i+1)+".  " + objectives[i].objectiveName;
			if (objectives [i].isComplete ()) {
				objectiveList += "\n **Completed**";

			}
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

    //public List<bool> getCurrentState()
    //{

    //}
}


