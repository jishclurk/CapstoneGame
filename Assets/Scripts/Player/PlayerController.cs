using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using LayerDefinitions;

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

    private PlayerAnimationController animController;
    private float animSpeed;
    private float walkSpeed;
   
    private bool selectingAbilityTarget = false;
    private IBasic activeBasicAbility;
    private ISpecial activeSpecialAbility;
    private CharacterAttributes attributes;
    private PlayerAbilities abilities;
    
    private PlayerResources resources;
    private HashSet<GameObject> watchedEnemies;
    private HashSet<GameObject> visibleEnemies;
    private TeamManager tm;

    private Transform eyes;

    private GameObject aoeArea;



    // Use this for initialization
    //things local to player go here
    private void Awake()
    {
        anim = GetComponent<Animator>();
        animController = GetComponent<PlayerAnimationController>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        eyes = transform.FindChild("Eyes");
        walkSpeed = 1.0f;

        //this is a mess. These are "shared" variables between co-op ai and player script
        Player player = GetComponent<Player>();
        abilities = player.abilities;
        activeBasicAbility = abilities.Basic;
        attributes = player.attributes;
        resources = player.resources;
        watchedEnemies = player.watchedEnemies;
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
            //Physics.Raycast(eyes.position, playerToTarget, out hit, 100f, Layers.NonPlayer) && hit.collider.gameObject.CompareTag("Enemy");
            if (Physics.Raycast(ray, out hit, 100f, Layers.NonWall))
            {
                HandleRayCastHit(hit);
            }
        }

        HandleAbilityInput();

        //enter combat pause


        if(activeSpecialAbility != null && activeSpecialAbility.GetAction() == AbilityHelper.Action.AimAOE)
        {
            HandleAbilityAim();
        }
        else if (activeSpecialAbility != null && activeSpecialAbility.GetAction() == AbilityHelper.Action.InheritTarget)
        {
            MoveAndShootSpecial();
        }
        else if (enemyClicked)
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
        if (remainingDistance >= activeBasicAbility.effectiveRange || !isTargetVisible(targetedEnemy))
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
            if (activeBasicAbility.isReady() && !targetIsDead)
            {
                activeBasicAbility.Execute(attributes, gameObject, targetedEnemy.gameObject);
                //if (!activeAbility.isbasicAttack) {
                //    activeAbility = abilities.Basic;
               // }
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

    private void MoveAndShootSpecial()
    {
        if (targetedEnemy == null)
        {
            //this return happens if enemy dies
            return; //avoid running code we don't need to.
        }
        navMeshAgent.destination = targetedEnemy.position;
        float remainingDistance = Vector3.Distance(targetedEnemy.position, transform.position);
        if (remainingDistance >= activeSpecialAbility.effectiveRange || !isTargetVisible(targetedEnemy))
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
            if (activeSpecialAbility.isReady() && !targetIsDead)
            {
                activeSpecialAbility.Execute(attributes, gameObject, targetedEnemy.gameObject);
                activeSpecialAbility = null;
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

        //Reset Current enemy target
        if (enemyClicked)
        {
            //activate enemy highlight on targetedEnemy.gameObject
        } else if (targetedEnemy != null) {
            tm.RemoveEnemyIfNotTeamVisible(targetedEnemy.gameObject);
            visibleEnemies.Remove(targetedEnemy.gameObject);
            targetedEnemy = null;
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
    
    private void useSpecialIfPossible(ISpecial ability)
    {
        //TODO: add debug/actual messages saying why ability failed
        if (ability.isReady() && resources.currentEnergy > ability.energyRequired)
        {
            if (ability.GetAction() == AbilityHelper.Action.NoTarget)
            {
                ability.Execute(attributes, gameObject, gameObject);
            }
            else if (ability.GetAction() == AbilityHelper.Action.AimAOE)
            {
                if (aoeArea != null)
                {
                    Destroy(aoeArea);
                }

                //enemyClicked = false;
                //friendClicked = false;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, 100f, Layers.NonWall))
                {
                    aoeArea = Instantiate(ability.aoeTarget, hit.point, Quaternion.Euler(-90, 0, 0)) as GameObject;
                } else
                {
                    aoeArea = Instantiate(ability.aoeTarget, transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject;
                }
                aoeArea.GetComponent<AOETargetController>().effectiveRange = ability.effectiveRange;
                navMeshAgent.Resume();
                activeSpecialAbility = ability;
            }
            else if (ability.GetAction() == AbilityHelper.Action.InheritTarget)
            {
                activeSpecialAbility = ability;
                //now MoveAndShoot with special (or error!)
            }
            
        }

    }

    private void HandleAbilityAim()
    {
        //walking animation stuff. 
        if(targetedEnemy != null)
        {
            navMeshAgent.destination = targetedEnemy.position;
            float remainingDistance = Vector3.Distance(targetedEnemy.position, transform.position);
            if (remainingDistance >= activeSpecialAbility.effectiveRange)
            {
                navMeshAgent.Resume();
                animSpeed = walkSpeed;
            } else
            {
                animSpeed = 0.0f;
                navMeshAgent.Stop();
            }
        } else
        {
            if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
                animSpeed = walkSpeed;
            else
                animSpeed = 0.0f;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            activeSpecialAbility.Execute(attributes, gameObject, aoeArea);
            aoeArea.GetComponent<AOETargetController>().enabled = false;
            aoeArea = null; //if switch player, aoeArea will be null so it will still exist if cast
            activeSpecialAbility = null;
        }

    }

    private bool isTargetVisible(Transform target)
    {
        RaycastHit hit;
        Vector3 playerToTarget = target.position - eyes.position;
        return Physics.Raycast(eyes.position, playerToTarget, out hit, 100f, Layers.NonPlayer) && hit.collider.gameObject.CompareTag("Enemy");

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
        activeBasicAbility = abilities.Basic;
        activeSpecialAbility = null;
        if(aoeArea != null)
        {
            Destroy(aoeArea);
        }
        
       
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

