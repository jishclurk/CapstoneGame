using System;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame{
	
	public class PlayerHandler : MonoBehaviour
	{
		public Player player;

        private ICommand move;

        void Start(){
            player = (Player)Instantiate(player);
            move = new MoveByAxis(player);
		}

		void Update()
		{
            move.Execute();
		}

        public Vector3 saveState()
        {
            return player.GetPosition();
        }

        public void setState(Vector3 position)
        {
            if(player == null)
            {
                player = (Player)Instantiate(player);
            }

            player.SetPosition(position);
        }

        public void Restart()
        {

        }

    }
}


