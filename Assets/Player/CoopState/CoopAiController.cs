﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using LayerDefinitions;

public class CoopAiController : MonoBehaviour {

    //members needed for functionality. This could get long? Separate scripts perhaps.
    //I put these here because state transitions shouldn't take parameters.
    [HideInInspector]
    public Animator anim;


    [HideInInspector]
    public NavMeshAgent navMeshAgent;

    //move state variables
    [HideInInspector]
    public float defaultFollowDist = 3.8f;
    [HideInInspector]
    public float followDist = 3.8f;
    [HideInInspector]
    public float randomDist = 0.8f; //followDist is + or - this value
    [HideInInspector]
    public float followEpsilon = 2.0f; //Determines how far player has to move for ai to start to follow again.
    [HideInInspector]
    public float chaseEpsilon = 2.0f;
    [HideInInspector]
    public float navSpeedDefault;

    //attack state variables
    [HideInInspector]
    public Collider sightCollider;
    [HideInInspector]
    public float sightDist = 10.0f;
    [HideInInspector]
    public HashSet<GameObject> watchedEnemies;
    [HideInInspector]
    public HashSet<GameObject> visibleEnemies;
    [HideInInspector]
    public ISpecial activeSpecialAbility;
    [HideInInspector]
    public Transform targetedEnemy;
    [HideInInspector]
    public Transform eyes;

    //attack prefs
    [HideInInspector]
    public enum TargetPref { Closest, Lowest, Active};
    public TargetPref targetChoose;

    //ability prefs
    //attack prefs
    [HideInInspector]
    public enum AbilityPref { Agressive, Offensive, Defensive, Balanced, None };
    public AbilityPref abilityChoose;


    [HideInInspector]
    public PlayerAbilities abilities;
    [HideInInspector]
    public Player player;
    [HideInInspector]
    public CharacterAttributes attributes;
    [HideInInspector]
    public TeamManager tm; //is this bad practice? Used to find which players are ai controlled or not
    [HideInInspector]
    public AbilityHelper ah;

    //Animation
    [HideInInspector]
    public PlayerAnimationController animController;
    [HideInInspector]
    public float animSpeed;
    [HideInInspector]
    public float walkSpeed;


    [HideInInspector] public ICoopState currentState;
    [HideInInspector] public IdleState idleState;
    [HideInInspector] public MoveState moveState;
    [HideInInspector] public AttackState attackState;
    [HideInInspector] public CastState castState;
    [HideInInspector]
    public FleeState fleeState;

    private void Awake()
    {
        idleState = new IdleState(this);
        moveState = new MoveState(this);
        attackState = new AttackState(this);
        castState = new CastState(this);
        fleeState = new FleeState(this);

        anim = GetComponent<Animator>();
        walkSpeed = 1.0f;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navSpeedDefault = navMeshAgent.speed;

        eyes = transform.FindChild("Eyes");
        sightCollider = GetComponent<SphereCollider>();
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
        ah = GameObject.FindWithTag("AbilityHelper").GetComponent<AbilityHelper>();

        //this is a mess. These are "shared" variables between co-op ai and player script
        player = GetComponent<Player>();
        animController = player.animController;
        abilities = player.abilities;
        attributes = player.attributes;
        watchedEnemies = player.watchedEnemies;
        visibleEnemies = player.visibleEnemies;
    }

    // Use this for initialization
    void Start()
    {
        currentState = idleState;
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();
    }


    public void CheckForCombat()
    {
        if (CheckLocalVision() || tm.visibleEnemies.Count > 0)
        {
            currentState = attackState;
            //attack priority moved to attackState
        }
    }

    public bool isTargetVisible(Transform target)
    {
        RaycastHit hit;
        Vector3 playerToTarget = target.position - eyes.position;
        return Physics.Raycast(eyes.position, playerToTarget, out hit, 100f, Layers.NonPlayer) && hit.collider.gameObject.CompareTag("Enemy");
    }

    //Reports if the aiPlayer has sight on at least one enemy
    public bool CheckLocalVision()
    {
        bool canSeeOneEnemy = false;
        foreach (GameObject enemy in watchedEnemies)
        {
            if (isTargetVisible(enemy.transform))
            {
                visibleEnemies.Add(enemy);
                tm.visibleEnemies.Add(enemy);
                canSeeOneEnemy = true;
            }
        }
        return canSeeOneEnemy;
    }

    //called by strategy when switching from ai -> player
    public void ResetOnSwitch()
    {
        targetedEnemy = null;
        currentState = idleState;
        navMeshAgent.speed = navSpeedDefault;
        if (abilities != null)
        {
            activeSpecialAbility = null;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger && other.tag.Equals("Enemy") && !other.GetComponent<EnemyHealth>().isDead)
        {
            watchedEnemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger && other.tag.Equals("Enemy"))
        {
            watchedEnemies.Remove(other.gameObject);
            visibleEnemies.Remove(other.gameObject);
            tm.RemoveEnemyIfNotTeamVisible(other.gameObject);
        }
    }



}