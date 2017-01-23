using System;
using System.Collections.Generic;
using UnityEngine;


namespace CapstoneGame{
	
	public interface IArtificialIntelligence
	{
		ICommand UpdateAI(IEntity entity);

		void AddEntityToWatch(IEntity entity);

		void ReleaseEntityToWatch(IEntity entity);

		List<IEntity> watchList ();
	}
}

