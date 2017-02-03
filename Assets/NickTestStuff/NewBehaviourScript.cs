using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : IStrategy {

    StatePatternPlayer player;

    public PlayerControl(StatePatternPlayer player)
    {
        this.player = player;
    }

	public void Update () {
        
	}
}
