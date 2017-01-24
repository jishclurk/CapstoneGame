using System;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame{
	
	public class PlayerHandler : MonoBehaviour
	{
		public Player player;

        //The different keys we need
        private ICommand move;

        void Start(){
            Debug.Log("player handler start");

            player = (Player)Instantiate(player);
            move = new MoveByAxis(player);
		}

		void Update()
		{
            move.Execute();
		}

		
	}
}


