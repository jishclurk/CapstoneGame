using System;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame
{
	public interface ICommand
	{
		 void Execute ();
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
            player.velocity = new Vector3(0, 1.0f, 0);
        }

	}

    public class MoveByAxis : ICommand
    {
        private IEntity player;

        public MoveByAxis(IEntity player)
        {
            this.player = player;
        }
        //called on key press
        public void Execute()
        {
            player.velocity = new Vector3(0, Input.GetAxis("Vertical"), 0);
        }
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
            player.velocity = new Vector3(0, -1.0f, 0);
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
   
}

