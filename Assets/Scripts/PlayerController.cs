﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{

    private Animator anim;
    private NavMeshAgent navMeshAgent;
    private Transform targetedEnemy;
    private Ray shootRay;
    private RaycastHit shootHit;
    private bool walking;
    private bool enemyClicked;
    private bool selectingAbilityTarget = false;
    private float nextFire;
    private IAbility activeAbility;
    private CharacterAttributes attributes;
    private PlayerAbilities abilities;


    // Use this for initialization
    void Awake()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        abilities = new PlayerAbilities();
        activeAbility = abilities.Basic;
        attributes = new CharacterAttributes();
    }


    // Update is called once per frame
    void Update()
    {


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetButtonDown("Fire2"))
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    targetedEnemy = hit.transform;
                    transform.LookAt(hit.transform); //prevents slow turn
                    enemyClicked = true;
                }
                else
                {
                    walking = true;
                    enemyClicked = false;
                    navMeshAgent.destination = hit.point;
                    transform.LookAt(hit.point); //prevents slow turn
                    navMeshAgent.Resume();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && abilities.Q.isReady())
        {
            enemyClicked = false; //unclick an enemy when an ability is pressed
            walking = false;
            activeAbility = abilities.Q;
            selectingAbilityTarget = true; //This variable needs an overhaul. I'm thinking it should work with a property of an ability. (instant fire, select enemy, select point on ground, select teammate)
        }

        if (enemyClicked)
        {
            MoveAndShoot();
        }
        else if(!selectingAbilityTarget)
        {
            walking = navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance;
        }

        anim.SetBool("Idling", !walking);
        anim.SetBool("NonCombat", !enemyClicked);
    }

    private void MoveAndShoot()
    {

        if (targetedEnemy == null)
        {
            //this return happens if enemy dies
            return; //avoid running code we don't need to.
        }
        navMeshAgent.destination = targetedEnemy.position;
        float remainingDistance = Vector3.Distance(targetedEnemy.position, transform.position);
        if (remainingDistance >= activeAbility.effectiveRange)
        {
            navMeshAgent.Resume();
            walking = true;
        }
        else
        {
            //Within range, look at enemy and shoot
            transform.LookAt(targetedEnemy);
            //Vector3 dirToShoot = targetedEnemy.transform.position - transform.position; //unused, would be for raycasting

            if (activeAbility.isReady())
            {
                activeAbility.Execute(attributes, gameObject, targetedEnemy.gameObject);
                if (!activeAbility.isbasicAttack) {
                    selectingAbilityTarget = false;
                    activeAbility = abilities.Basic;
                }
            }

            navMeshAgent.Stop(); //within range, stop moving
            walking = false;
        }


    }
}

