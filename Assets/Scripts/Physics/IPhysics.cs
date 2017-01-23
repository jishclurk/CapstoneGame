using System;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame
{
	public interface IPhysics
	{

		void UpdateSpeed(float speed);

		void UpdateVelocity(Vector3 velocity);

		void UpdatePosition ();

	}
}

