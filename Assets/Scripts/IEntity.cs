using System;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame
{
	public interface IEntity
	{

		void UpdatePhysics(Vector3 velocity);
		Command UpdateAI ();
		bool BeingWatched ();
		void SetWatched (bool watched);
		Transform GetTransform();
	}
}

