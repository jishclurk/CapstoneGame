using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public CharacterAttributes attributes;
    public PlayerResources resources;
    public Strategy strategy;
    public PlayerAbilities abilities;

	// Use this for initialization
	void Awake () {
        attributes = GetComponent<CharacterAttributes>();
        resources = GetComponent<PlayerResources>();
        strategy = GetComponent<Strategy>();
        abilities = GetComponent<PlayerAbilities>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
