using System;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame{
	
	public class PlayerHandler : MonoBehaviour
	{

		//ATTACH GAME OBJECT TO PLAYER!!!!

		public Player player;

        //Object that moves based on input
		public GameObject playerObject;

        //The different keys we need
        private ICommand move;

        void Start(){
			player = new Player (playerObject);
            move = new Move(player);

			//buttonUp = new MoveUp (player);
			//buttonDown = new MoveDown (player);
		}

		void Update()
		{
            move.Execute();
			//HandleInput ();
		}

		//public void HandleInput(){

		//	if (Input.GetKey(KeyCode.UpArrow)) {
		//		buttonUp.Execute ();
		//	} else if (Input.GetKey(KeyCode.DownArrow)) {
		//		buttonDown.Execute ();
		//	}

		//}

	}
}


