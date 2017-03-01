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
    [HideInInspector]
    public HashSet<GameObject> watchedEnemies;
    [HideInInspector]
    public HashSet<GameObject> visibleEnemies;
    [HideInInspector]
    public Transform gunbarrel;


    public int id;

    // Use this for initialization
    void Awake () {
        attributes = GetComponent<CharacterAttributes>();
        abilities = GetComponent<PlayerAbilities>();
        resources = GetComponent<PlayerResources>();
        strategy = GetComponent<Strategy>();

        watchedEnemies = new HashSet<GameObject>();
        visibleEnemies = new HashSet<GameObject>();
        gunbarrel = transform.FindDeepChild("ShootFX");
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log( gameObject.name + " Visible enemy Count:" + visibleEnemies.Count);
	}
}
