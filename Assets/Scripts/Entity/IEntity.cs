using System;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame
{
	public interface IEntity
	{
        //??

        Vector3 velocity { get; set; }

		//void UpdatePhysics(Vector3 velocity);
		//ICommand UpdateAI (IEntity entity);
		//bool BeingWatched ();
		//void SetWatched (bool watched);
		Vector3 GetPosition();
	}
}

