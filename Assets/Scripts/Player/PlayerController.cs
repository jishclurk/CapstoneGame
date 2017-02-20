﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public CombatPause combatPause;

    private Animator anim;
    private NavMeshAgent navMeshAgent;

    //public things that need to be changed on strategy switch
    [HideInInspector]
    public Transform targetedEnemy;
    [HideInInspector]
    public bool enemyClicked; //Team manager accesses this to determine if player is in combat

    private Transform targetedFriend;
    private bool friendClicked;

    private Ray shootRay;
    private RaycastHit shootHit;

    private PlayerAnimationController animController;
    private float animSpeed;
    private float walkSpeed;
   
    private bool selectingAbilityTarget = false;
    private IAbility activeAbility;
    private CharacterAttributes attributes;
    private PlayerAbilities abilities;
    
    private PlayerResources resources;
    private HashSet<GameObject> watchedEnemies;
    private HashSet<GameObject> visibleEnemies;
    private TeamManager tm;

    private Transform eyes;

    private GameObject aoeArea;

    private SimpleGameManager gm;


    // Use this for initialization
    //things local to player go here
    private void Awake()
    {
        combatPause = Instantiate(combatPause) as CombatPause; 
        anim = GetComponent<Animator>();
        animController = GetComponent<PlayerAnimationController>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        eyes = transform.FindChild("Eyes");
        walkSpeed = 1.0f;

        //this is a mess. These are "shared" variables between co-op ai and player script
        Player player = GetComponent<Player>();
        abilities = player.abilities;
        activeAbility = abilities.Basic;
        attributes = player.attributes;
        resources = player.resources;
        watchedEnemies = player.watchedEnemies;
        gm = SimpleGameManager.Instance;
        visibleEnemies = player.visibleEnemies;
    }

    //things from other scripts go here
    void Start()
    {
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
		tm.playerResources = resources;
    }


    // Update is called once per frame
    void Update()
    {
        //Player click input
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetButtonDown("Fire2"))
        {
            if (Physics.Raycast(ray, out hit))
            {
                HandleRayCastHit(hit);
            }
        }
        if (activeAbility.requiresAim)
        {
            enemyClicked = false; //This line allows the player to still click on floor and move from above HandleRayCastHit, 
                                    //but any targets clicked will not be recognized. Stops active ability from switching to basic and still having AOE circle. can be changed
            HandleAbilityAim();
        }

        HandleAbilityInput();

        //enter combat pause
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gm.gameState.Equals(GameState.COMABT_PAUSE))
            {
                gm.OnStateChange += combatPause.Disable;
                gm.SetGameState(GameState.PLAY);
            }else
            {
                gm.OnStateChange += combatPause.Enable;
                gm.SetGameState(GameState.COMABT_PAUSE);
            }

        }

        if (enemyClicked)
        {
            MoveAndShoot();
        }
        else
        {
            if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
                animSpeed = walkSpeed;
            else
                animSpeed = 0.0f;
        }

        //temporary fix
        if (animSpeed > 0.0f)
            animController.AnimateMovement(animSpeed);
        else
            animController.AnimateIdle();
        //anim.SetBool("NonCombat", !tm.IsTeamInCombat());
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
        if (remainingDistance >= activeAbility.effectiveRange || !isTargetVisible(targetedEnemy))
        {
            navMeshAgent.Resume();
            animSpeed = walkSpeed;
        }
        else
        {
            //Within range, look at enemy and shoot
            transform.LookAt(targetedEnemy);
            animController.AnimateAim();

            bool targetIsDead = targetedEnemy.GetComponent<EnemyHealth>().isDead;
            if (activeAbility.isReady() && !targetIsDead)
            {
                activeAbility.Execute(attributes, gameObject, targetedEnemy.gameObject);
                if (!activeAbility.isbasicAttack) {
                    //selectingAbilityTarget = false;
                    activeAbility = abilities.Basic;
                }
            }

            navMeshAgent.Stop(); //within range, stop moving
            if (targetIsDead)
            {
                enemyClicked = false;
                tm.RemoveDeadEnemy(targetedEnemy.gameObject);
                navMeshAgent.destination = transform.position;
            }
            animSpeed = 0.0f;
        }


    }

    private void HandleRayCastHit(RaycastHit hit)
    {
        //Reset Current enemy target
        if (targetedEnemy != null)
        {
            tm.RemoveEnemyIfNotTeamVisible(targetedEnemy.gameObject);
            visibleEnemies.Remove(targetedEnemy.gameObject);
            targetedEnemy = null;
        }

        if (hit.collider.CompareTag("Enemy"))
        {
            //target is instead the raycast hit, rather than a transform. Put work into Execute().
            targetedEnemy = hit.transform;
            transform.LookAt(hit.transform); //prevents slow turn
            enemyClicked = true;
            friendClicked = false;

            //update combat
            visibleEnemies.Add(targetedEnemy.gameObject);
            tm.visibleEnemies.Add(targetedEnemy.gameObject);
            watchedEnemies.Add(targetedEnemy.gameObject);
        }
        else if (hit.collider.CompareTag("Player"))
        {
            targetedFriend = hit.transform;
            transform.LookAt(hit.transform); //prevents slow turn
            friendClicked = true;
            enemyClicked = false;
        }
        else
        {
            animSpeed = walkSpeed;
            enemyClicked = false;
            friendClicked = false;
            navMeshAgent.destination = hit.point;
            transform.LookAt(new Vector3(hit.point.x, gameObject.transform.position.y, hit.point.z));
            navMeshAgent.Resume();
        }
    }

    private void HandleAbilityInput()
    {

        foreach (KeyCode key in abilities.AbilityBindings.Keys)
        {
            if (Input.GetKeyDown(key))
            {
                useSpecialIfPossible(abilities.abilityArray[abilities.AbilityBindings[key]]);
            }
        }
        //Handle ability input
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    useSpecialIfPossible(abilities.one);
        //}
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    useSpecialIfPossible(abilities.two);
        //}
    }
    
    private void useSpecialIfPossible(IAbility ability)
    {
        //TODO: add debug/actual messages saying why ability failed
        if (ability.isReady() && resources.currentEnergy > ability.energyRequired)
        {
            if (!ability.requiresTarget)
            {
                ability.Execute(attributes, gameObject, gameObject);
            }
            else if (!activeAbility.requiresAim && ability.requiresAim)
            {
                enemyClicked = false;
                friendClicked = false;
                aoeArea = Instantiate(ability.aoeTarget) as GameObject;
                aoeArea.GetComponent<AOETargetController>().effectiveRange = ability.effectiveRange;
                activeAbility = ability;
            }
            else if (ability.requiresTarget && (friendClicked || enemyClicked))
            {
                activeAbility = ability;
            }
            
        }

    }

    private void HandleAbilityAim()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            activeAbility.Execute(attributes, gameObject, aoeArea);
            aoeArea.GetComponent<AOETargetController>().enabled = false;
            Destroy(aoeArea, activeAbility.timeToCast);
            activeAbility = abilities.Basic;
        }

    }

    private bool isTargetVisible(Transform target)
    {
        RaycastHit hit;
        Vector3 playerToTarget = target.position - eyes.position;
        return Physics.Raycast(eyes.position, playerToTarget, out hit) && hit.collider.gameObject.CompareTag("Enemy");

    }

    public void ResetOnSwitch()
    {
        targetedEnemy = null;
        navMeshAgent.destination = transform.position;
        enemyClicked = false;
        targetedFriend = null;
        friendClicked = false;
        animSpeed = 0.0f;
        selectingAbilityTarget = false;
        activeAbility = abilities.Basic;
        Destroy(aoeArea);
       
    }

    //update shared watchedEnemies between co-op and ai
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
            //Debug.Log("OnTriggerExit: " + (tm == null));
            if (tm != null)
            { //WTF I have no idea how teamManager becomes null here but it does. seriously wtf
                //Happens when AI is attacking an enemy. You are not, but are being chased by 2nd enemy. AI's enemy exits range
                tm.RemoveEnemyIfNotTeamVisible(other.gameObject);
            }
        }
    }

}

