using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{

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

    private bool walking;
   
    private bool selectingAbilityTarget = false;
    private IAbility activeAbility;
    private CharacterAttributes attributes;
    private PlayerAbilities abilities;
    
    private PlayerResources resources;
    private List<GameObject> watchedEnemies;
    private TeamManager tm;

    private Transform eyes;


    // Use this for initialization
    //things local to player go here
    private void Awake()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        eyes = transform.FindChild("Eyes");

        //this is a mess. These are "shared" variables between co-op ai and player script
        Player player = GetComponent<Player>();
        abilities = player.abilities;
        activeAbility = abilities.Basic;
        attributes = player.attributes;
        resources = player.resources;
        watchedEnemies = player.watchedEnemies;
        Debug.Log("PC start we: " + watchedEnemies);
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
        if(tm.IsTeamInCombat())
        {
            navMeshAgent.speed = 5.5f;
        }
        else
        {
            navMeshAgent.speed = 5.4f;
        }
        anim.SetBool("Idling", !walking);
        anim.SetBool("NonCombat", !tm.IsTeamInCombat());
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
            walking = true;
        }
        else
        {
            //Within range, look at enemy and shoot
            transform.LookAt(targetedEnemy);

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
            walking = false;
        }


    }

    private void HandleRayCastHit(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Enemy"))
        {
            //target is instead the raycast hit, rather than a transform. Put work into Execute().
            targetedEnemy = hit.transform;
            transform.LookAt(hit.transform); //prevents slow turn
            enemyClicked = true;
            friendClicked = false;
            if (!tm.visibleEnemies.Contains(targetedEnemy.gameObject))
            {
                tm.visibleEnemies.Add(targetedEnemy.gameObject);
            }
            if (!watchedEnemies.Contains(targetedEnemy.gameObject))
            {
                watchedEnemies.Add(targetedEnemy.gameObject);
            }
        }
        else if (hit.collider.CompareTag("Player"))
        {
            if (targetedEnemy != null)
            {
                tm.RemoveEnemyIfNotTeamVisible(targetedEnemy.gameObject);
            }
            targetedFriend = hit.transform;
            transform.LookAt(hit.transform); //prevents slow turn
            friendClicked = true;
            enemyClicked = false;
        }
        else
        {
            if (targetedEnemy != null)
            {
                tm.RemoveEnemyIfNotTeamVisible(targetedEnemy.gameObject);
            }
            walking = true;
            enemyClicked = false;
            friendClicked = false;
            navMeshAgent.destination = hit.point;
            transform.LookAt(new Vector3(hit.point.x, gameObject.transform.position.y, hit.point.z));
            navMeshAgent.Resume();
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

    public void ResetOnSwitch()
    {
        targetedEnemy = null;
        navMeshAgent.destination = transform.position;
        enemyClicked = false;
        targetedFriend = null;
        friendClicked = false;
        walking = false;
        selectingAbilityTarget = false;
        activeAbility = abilities.Basic;
       
    }


    //update shared watchedEnemies between co-op and ai
    private void OnTriggerEnter(Collider other)
    {
       
        if (!other.isTrigger && other.tag.Equals("Enemy"))
        {
            Debug.Log(watchedEnemies);
            if (!watchedEnemies.Contains(other.gameObject))
            {
                watchedEnemies.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger && other.tag.Equals("Enemy"))
        {
            watchedEnemies.Remove(other.gameObject);
            tm.RemoveEnemyIfNotTeamVisible(other.gameObject);
        }
    }

}

