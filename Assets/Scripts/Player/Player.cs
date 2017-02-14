using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [HideInInspector]
    public CharacterAttributes attributes;
    [HideInInspector]
    public PlayerResources resources;
    [HideInInspector]
    public Strategy strategy;
    [HideInInspector]
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
