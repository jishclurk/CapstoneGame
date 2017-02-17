using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class that ensures that persisitance objects, members of game, and not deleted between scenes 
public class DDOL : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (this);
	}

    private void Start()
    {
        if(SimpleGameManager.Instance.level == 0)
        {
            SimpleGameManager.Instance.NewGame();
        }
    }

	

}
