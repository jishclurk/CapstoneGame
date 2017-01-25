using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CapstoneGame
{
	public interface ICommand
	{
		 void Execute ();
	}

	public class MoveUp : ICommand
	{
        private IEntity player;
		
        public MoveUp(IEntity player)
        {
            this.player = player;
        }
        
		public void Execute()
		{
            player.velocity = new Vector3(0, 1.0f, 0);
        }

	}

    //When excuted, moves player based on the input from the verticle axis
    public class MoveByAxis : ICommand
    {
        private IEntity player;

        public MoveByAxis(IEntity player)
        {
            this.player = player;
        }

        public void Execute()
        {
            player.velocity = new Vector3(0, Input.GetAxis("Vertical"), 0);
        }
    }

    public class MoveDown : ICommand
	{
        private IEntity player;

        public MoveDown(IEntity player)
        {
            this.player = player;
        }

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
        GameManager gm;

        public Pause(GameManager gm)
        {
            this.gm = gm;
        }
        public void Execute()
        {
            Time.timeScale = 0;
            gm.LoadPauseScreen();
        }
    }

    public class Resume : ICommand
    {
        GameManager gm;

        public Resume(GameManager gm)
        {
            this.gm = gm;
        }
        public void Execute()
        {
            //Count down??
            Time.timeScale = 1;
            gm.ResumeGame();
        }
    }

    public class ToMenu : ICommand
    {
        public void Execute()
        {
            SceneManager.LoadScene("Pong Menu");
        }
    }

    public class Exit : ICommand
    {
        public void Execute()
        {
            
        }
    }
   
}

