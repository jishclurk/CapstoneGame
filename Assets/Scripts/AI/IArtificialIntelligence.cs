using System;
using System.Collections.Generic;
using UnityEngine;


namespace CapstoneGame{
	
	public interface IArtificialIntelligence
	{

		ICommand UpdateAI(IEntity entity);

	}
}

