using System;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame
{
	public interface ICommand
	{

		//move command
		 void Execute ();

		//Move player in certain direction
		// void Move (IEntity player);
	}

	public class MoveUp :ICommand
	{
        private IEntity player;
		
        public MoveUp(IEntity player)
        {
            this.player = player;
        }
        
        //called on key press
		public void Execute()
		{
			Move(player);

		}

		public void Move(IEntity player){
			player.UpdatePhysics (new Vector3(0,1.0f,0 ));
//			playerTrans.Translate (playerTrans.up * speed);
//			if (playerTrans.position.y > 5.5f) {
//				playerTrans.position = new Vector3 (15.0f, 5.5f, 0.0f);
//			}

		}
	}

    public class Move : ICommand
    {
        private IEntity player;

        public Move(IEntity player)
        {
            this.player = player;
        }
        //called on key press
        public void Execute()
        {
            player.UpdatePhysics(new Vector3(0, Input.GetAxis("Vertical"), 0));
            //player.UpdatePhysics(new Vector3(0, -1.0f, 0));
           // Move(player);

        }

        //public void Move(IEntity player)
        //{
        //    player.UpdatePhysics(new Vector3(0, -1.0f, 0));

        //}
    }

    public class MoveDown :ICommand
	{
        private IEntity player;

        public MoveDown(IEntity player)
        {
            this.player = player;
        }
        //called on key press
        public void Execute()
		{
			Move(player);

		}

		public void Move(IEntity player){
			player.UpdatePhysics (new Vector3(0,-1.0f,0 ));

		}
	}
		
	public class DoNothing : ICommand
	{
		//Called when we press a key
		public void Execute()
		{
			//Nothing will happen if we press this key
		}

		public void Move(IEntity player)
		{
			//Nothing will happen if we press this key
		}
	}

    public class Pause : ICommand
    {
        //Called when we press a key
        public void Execute()
        {
            //IMPLEMENT
        }

        public void Move(IEntity player)
        {
            //IMPLEMENT

        }
    }

    public class Play : ICommand
    {
        //Called when we press a key
        public void Execute()
        {
            //IMPLEMENT

        }

        public void Move(IEntity player)
        {
            //IMPLEMENT

        }
    }
    public class Exit : ICommand
    {
        //Called when we press a key
        public void Execute()
        {
            //IMPLEMENT
        }

        public void Move(IEntity player)
        {
            //IMPLEMENT
        }
    }
    //	public class EnemyMoveUp :Command
    //	{
    //		//called on key press
    //		public override void Execute(Transform playerTrans, float speed)
    //		{
    //			Move(playerTrans,speed);
    //
    //		}
    //
    //		public override void Move(Transform playerTrans,float speed){
    //			playerTrans.Translate (0.0f, speed, 0.0f);
    //		}
    //	}
    //
    //	public class EnemyMoveDown :Command
    //	{
    //		//called on key press
    //		public override void Execute(Transform playerTrans, float speed)
    //		{
    //			Move(playerTrans,speed);
    //
    //		}
    //
    //		public override void Move(Transform playerTrans,float speed){
    //			playerTrans.Translate (0.0f, -speed, 0.0f);
    //		}
    //	}
}

