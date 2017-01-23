using System;
using System.Collections.Generic;
using UnityEngine;


namespace CapstoneGame{
	
	public interface IArtificialIntelligence
	{
		Command UpdateAI();

		void AddEntityToWatch(IEntity entity);

		void ReleaseEntityToWatch(IEntity entity);
	}
}

