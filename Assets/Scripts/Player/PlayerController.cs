using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{

    private Animator anim;
    private NavMeshAgent navMeshAgent;
    private Transform target;
    public bool enemyClicked; //Team manager accesses this to determine if player is in combat
    public bool enemyEngaged;
    private Transform targetedFriend;
    private bool friendClicked;

    private Ray shootRay;
    private RaycastHit shootHit;

    private bool walking;
   
    private bool selectingAbilityTarget = false;
    private IAbility activeAbility;
    private CharacterAttributes attributes;
    private PlayerAbilities abilities;
    private TeamManager tm;
    private PlayerResources resources;
    private Transform eyes;


    // Use this for initialization
    void Awake()
    {
        anim = GetComponent<Animator>();
        Player player = GetComponent<Player>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        abilities = player.abilities;
        activeAbility = abilities.Basic;
        attributes = player.attributes;
        resources = player.resources;
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
		tm.playerResources = resources;
        eyes = transform.FindChild("Eyes");
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
                if (hit.collider.CompareTag("Enemy"))
                {
                    //target is instead the raycast hit, rather than a transform. Put work into Execute().
                    target = hit.transform;
                    transform.LookAt(hit.transform); //prevents slow turn
                    enemyClicked = true;
                    enemyEngaged = true;
                    friendClicked = false;
                    if (!tm.visibleEnemies.Contains(target.gameObject))
                    {
                        tm.visibleEnemies.Add(target.gameObject);
                    }
                }
                else if (hit.collider.CompareTag("Player"))
                {
                    //target is instead the raycast hit, rather than a transform. Put work into Execute().
                    target = hit.transform;
                    transform.LookAt(hit.transform); //prevents slow turn
                    friendClicked = true;
                    enemyClicked = false;
                }
                else
                {
                    walking = true;
                    enemyClicked = false;
                    friendClicked = false;
                    navMeshAgent.destination = hit.point;
                    transform.LookAt(new Vector3(hit.point.x, gameObject.transform.position.y, hit.point.z));
                    navMeshAgent.Resume();
                }
            }
        }

        HandleAbilityInput();

        if (enemyClicked)
        {
            MoveAndShoot();
        }
        else
        {
            walking = navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance;
        }

        //temporary fix
        if(tm.isTeamInCombat())
        {
            navMeshAgent.speed = 5.5f;
        }
        else
        {
            navMeshAgent.speed = 4.5f;
        }
        anim.SetBool("Idling", !walking);
        anim.SetBool("NonCombat", !tm.isTeamInCombat());
    }

    private void MoveAndShoot()
    {

        if (target == null)
        {
            //this return happens if enemy dies
            return; //avoid running code we don't need to.
        }
        navMeshAgent.destination = target.position;
        float remainingDistance = Vector3.Distance(target.position, transform.position);
        if (remainingDistance >= activeAbility.effectiveRange || !isTargetVisible(target))
        {
            navMeshAgent.Resume();
            walking = true;
        }
        else
        {
            //Within range, look at enemy and shoot
            transform.LookAt(target);

            bool targetIsDead = target.GetComponent<EnemyHealth>().isDead;
            if (activeAbility.isReady() && !targetIsDead)
            {
                activeAbility.Execute(attributes, gameObject, target.gameObject);
                if (!activeAbility.isbasicAttack) {
                    //selectingAbilityTarget = false;
                    activeAbility = abilities.Basic;
                }
            }

            navMeshAgent.Stop(); //within range, stop moving
            if (targetIsDead)
            {
                enemyEngaged = false;
                enemyClicked = false;
                tm.visibleEnemies.Remove(target.gameObject);
                navMeshAgent.destination = transform.position;
            }
            walking = false;
        }


    }

    private void HandleAbilityInput()
    {
        //Handle ability input
        if (Input.GetKeyDown(KeyCode.Q))
        {
            useSpecialIfPossible(abilities.Q);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            useSpecialIfPossible(abilities.W);
        }
    }
    
    private void useSpecialIfPossible(IAbility ability)
    {
        //TODO: add debug/actual messages saying why ability failed
        if (ability.isReady() && resources.currentEnergy > ability.energyRequired)
        {
            if (ability.requiresTarget && (friendClicked || enemyClicked))
            {
                activeAbility = ability;
            }
            if (!ability.requiresTarget)
            {
                ability.Execute(attributes, gameObject, gameObject);
            }
        }

    }

    private bool isTargetVisible(Transform target)
    {
        RaycastHit hit;
        Vector3 playerToTarget = target.position - eyes.position;
        return Physics.Raycast(eyes.position, playerToTarget, out hit) && hit.collider.gameObject.CompareTag("Enemy");

    }

}

