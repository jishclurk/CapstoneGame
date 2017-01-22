using System;
using System.Collections.Generic;
using UnityEngine;

namespace CapstoneGame
{
	public abstract class Command
	{



		//move command
		public abstract void Execute (IEntity player);

		//Move player in certain direction
		public abstract void Move (IEntity player);
	}

	public class MoveUp :Command
	{
		//called on key press
		public override void Execute(IEntity player)
		{
			Move(player);

		}

		public override void Move(IEntity player){
			player.UpdatePhysics (new Vector3(0,1.0f,0 ));
//			playerTrans.Translate (playerTrans.up * speed);
//			if (playerTrans.position.y > 5.5f) {
//				playerTrans.position = new Vector3 (15.0f, 5.5f, 0.0f);
//			}

		}
	}

	public class MoveDown :Command
	{
		//called on key press
		public override void Execute(IEntity player)
		{
			Move(player);

		}

		public override void Move(IEntity player){
			player.UpdatePhysics (new Vector3(0,-1.0f,0 ));

		}
	}
		
	public class DoNothing : Command
	{
		//Called when we press a key
		public override void Execute(IEntity player)
		{
			//Nothing will happen if we press this key
		}

		public override void Move(IEntity player)
		{
			//Nothing will happen if we press this key
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

