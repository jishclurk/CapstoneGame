using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IGoal 
	{
		string GoalText { get; set;}
		TeamManager TeamM { get; set;}
		bool IsAchieved();
		void Complete();
		void DestroyGoal ();

	}


