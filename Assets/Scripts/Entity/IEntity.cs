using System;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame
{
	public interface IEntity
	{
        Vector3 velocity { get; set; }

		Vector3 GetPosition();
	}
}

