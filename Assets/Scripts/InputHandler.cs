using System;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame{
	
	public class InputHandler : MonoBehaviour
	{


		//ATTACH GAME OBJECT TO PLAYER!!!!

		public Player player;
		public GameObject playerObject;
		//The different keys we need
		private Command buttonUp, buttonDown;

		void Start(){

			player = new Player (playerObject);

			buttonUp = new MoveUp ();
			buttonDown = new MoveDown ();

		}

		void Update()
		{

			HandleInput ();

		}

		public void HandleInput(){

			if (Input.GetKey(KeyCode.UpArrow)) {
				buttonUp.Execute (player);
			} else if (Input.GetKey(KeyCode.DownArrow)) {
				buttonDown.Execute (player);
			}

		}

	}
}


