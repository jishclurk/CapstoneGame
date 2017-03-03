using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using LayerDefinitions;

public class PlayerController : MonoBehaviour
{

    //private Animator anim;
    private NavMeshAgent navMeshAgent;

    //public things that need to be changed on strategy switch
    [HideInInspector]
    public Transform targetedEnemy;

    public bool changeTargetOnSpecial = false;

    private Transform specialTargetedEnemy;

    private Transform targetedFriend;
    private bool friendClicked;

    private Ray shootRay;
    private RaycastHit shootHit;

    private PlayerAnimationController animController;
    private float animSpeed;
    private float walkSpeed;
    private float chaseEpsilon;
    private float navSpeedDefault;
   
    private IBasic activeBasicAbility;
    private ISpecial activeSpecialAbility;
    private Player player;
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
        //anim = GetComponent<Animator>();
        animController = GetComponent<PlayerAnimationController>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        eyes = transform.FindChild("Eyes");
        walkSpeed = 1.0f;
        chaseEpsilon = 2.0f;
        navSpeedDefault = navMeshAgent.speed;

        //this is a mess. These are "shared" variables between co-op ai and player script
        player = GetComponent<Player>();
        abilities = player.abilities;
        activeBasicAbility = abilities.Basic;
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
        //PLAYER INPUT//
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetButton("Fire2"))
        {
            //Physics.Raycast(eyes.position, playerToTarget, out hit, 100f, Layers.NonPlayer) && hit.collider.gameObject.CompareTag("Enemy");
            if (Physics.Raycast(ray, out hit, 100f, Layers.NonWall))
            {
                HandleRayCastHit(hit);
            }
        }

        HandleAbilityInput();

        //PLAYER ACTION//
        if (activeSpecialAbility != null && activeSpecialAbility.GetAction() == AbilityHelper.Action.AOE)
        {
            AimAOESpecial();
        }
        else if (activeSpecialAbility != null && activeSpecialAbility.GetAction() == AbilityHelper.Action.Target)
        {
            if (specialTargetedEnemy == null)
                AimTargetSpecial();
            else
                ShootSpecialOnTarget(specialTargetedEnemy);
        }
        else if (targetedEnemy != null)
        {
            ShootGun();
        }

        //PLAYER ANIMATION//
        if(specialTargetedEnemy != null)
        {
            HandleDestinationAnimation(specialTargetedEnemy, activeSpecialAbility);
        } else
        {
            HandleDestinationAnimation(targetedEnemy, activeBasicAbility);
        }
       

    }

    ////////////////////////
    //INPUT HELPER METHODS//
    ////////////////////////
    private void HandleRayCastHit(RaycastHit hit)
    {
        bool enemyClicked = false;
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
            navMeshAgent.destination = transform.position; //temporary, can add functionality to set teammate as destination for healing/buff abilities later
            friendClicked = true;
        }
        else
        {
            animSpeed = walkSpeed;
            friendClicked = false;
            navMeshAgent.destination = hit.point;
            transform.LookAt(new Vector3(hit.point.x, gameObject.transform.position.y, hit.point.z));
            navMeshAgent.Resume();
        }

        if (!enemyClicked && targetedEnemy != null)
        {
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
                Debug.Log(abilities.AbilityBindings[key]);
                Debug.Log(abilities.abilityArray[abilities.AbilityBindings[key]]);
                if (activeSpecialAbility == null)
                {

                    useSpecialIfPossible(abilities.abilityArray[abilities.AbilityBindings[key]]);
                }
                else
                {
                    
                    if (!activeSpecialAbility.Equals(abilities.abilityArray[abilities.AbilityBindings[key]]))
                    {
                        cancelSpecialIfPossible(activeSpecialAbility);
                        useSpecialIfPossible(abilities.abilityArray[abilities.AbilityBindings[key]]);
                    } else
                    {
                        cancelSpecialIfPossible(activeSpecialAbility);
                    }
                    
                }
            }
        }
    }

    private void cancelSpecialIfPossible(ISpecial ability)
    {
        if (ability.GetAction() == AbilityHelper.Action.AOE)
        {
            if (aoeArea != null)
            {
                Destroy(aoeArea);
                aoeArea = null;
                return;
            }
        }

        activeSpecialAbility = null;

    }

    private void useSpecialIfPossible(ISpecial ability)
    {
        //TODO: add debug/actual messages saying why ability failed
        if (ability.isReady() && resources.currentEnergy > ability.energyRequired)
        {
            if (ability.GetAction() == AbilityHelper.Action.Instant)
            {
                ability.Execute(player, gameObject, gameObject);
            }
            else if (ability.GetAction() == AbilityHelper.Action.AOE)
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f, Layers.NonWall))
                {
                    aoeArea = Instantiate(ability.aoeTarget, hit.point, Quaternion.Euler(-90, 0, 0)) as GameObject;
                }
                else
                {
                    aoeArea = Instantiate(ability.aoeTarget, transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject;
                }
                aoeArea.GetComponent<AOETargetController>().effectiveRange = ability.effectiveRange;
                navMeshAgent.Resume();
                activeSpecialAbility = ability;
                //AimAOE
            }
            else if (ability.GetAction() == AbilityHelper.Action.Target)
            {
                activeSpecialAbility = ability;
                //AimTarget
            }
        }
    }

    /////////////////////////
    //ACTION HELPER METHODS//
    /////////////////////////
    private void ShootGun()
    {
        if (targetedEnemy == null)
        {
            return;
        }
        float remainingDistance = Vector3.Distance(targetedEnemy.position, transform.position);
        if (remainingDistance <= activeBasicAbility.effectiveRange && isTargetVisible(targetedEnemy))
        {
            //Within range, look at enemy and shoot
            transform.LookAt(targetedEnemy);
            //animController.AnimateAimStanding();

            bool targetIsDead = targetedEnemy.GetComponent<EnemyHealth>().isDead;
            if (activeBasicAbility.isReady() && !targetIsDead)
            {
                animController.AnimateShoot();
                activeBasicAbility.Execute(player, gameObject, targetedEnemy.gameObject);
            }
            if (targetIsDead)
            {
                tm.RemoveDeadEnemy(targetedEnemy.gameObject);
                navMeshAgent.destination = transform.position;
            }
            animSpeed = 0.0f;
        }
    }

    //Look for left click on enemy
    private void AimTargetSpecial()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetButtonDown("Fire1"))
        {
            if (Physics.Raycast(ray, out hit, 100f, Layers.Enemy))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    specialTargetedEnemy = hit.transform;
                    if (changeTargetOnSpecial)
                    {
                        targetedEnemy = specialTargetedEnemy;
                        visibleEnemies.Add(targetedEnemy.gameObject);
                        tm.visibleEnemies.Add(targetedEnemy.gameObject);
                        watchedEnemies.Add(targetedEnemy.gameObject);
                    }

                }
            }
            else
            { //if you miss an enemy with click, activate on current target if exists
                if (targetedEnemy != null)
                {
                    specialTargetedEnemy = targetedEnemy;
                }
            }
        } 
    }

    private void ShootSpecialOnTarget(Transform target)
    {
        if (target == null)
        {
            return;
        }
        float remainingDistance = Vector3.Distance(target.position, transform.position);
        if (remainingDistance <= activeSpecialAbility.effectiveRange && isTargetVisible(target))
        {
            //Within range, look at enemy and shoot
            transform.LookAt(target);
            //animController.AnimateAimStanding();

            bool targetIsDead = target.GetComponent<EnemyHealth>().isDead;
            if (activeSpecialAbility.isReady() && !targetIsDead)
            {
                activeSpecialAbility.Execute(player, gameObject, target.gameObject);
                activeSpecialAbility = null;
                specialTargetedEnemy = null;
            }
            if (targetIsDead)
            {
                tm.RemoveDeadEnemy(target.gameObject);
                if (changeTargetOnSpecial)
                {
                    targetedEnemy = null;
                }
            }
            navMeshAgent.destination = transform.position;
            animSpeed = 0.0f;
        }
    }

    private void AimAOESpecial()
    {
        //raycasting is handled by script attached to aoeArea
        if (Input.GetButtonDown("Fire1"))
        {
            activeSpecialAbility.Execute(player, gameObject, aoeArea);
            aoeArea.GetComponent<AOETargetController>().enabled = false;
            aoeArea = null; //if switch player, aoeArea will be null so it will still exist if cast
            activeSpecialAbility = null;
        }

    }

    private void HandleDestinationAnimation(Transform target, IAbility ability)
    {
        if (target != null)
        {
            navMeshAgent.destination = target.position;
            float remainingDistance = Vector3.Distance(target.position, transform.position);
            if (remainingDistance >= ability.effectiveRange)
            {
                animController.AnimateAimChasing();
                navMeshAgent.speed = navSpeedDefault;
                navMeshAgent.Resume();
            } else if (remainingDistance >= ability.effectiveRange - chaseEpsilon)
            {
                animController.AnimateAimChasing();
                navMeshAgent.speed = target.GetComponent<NavMeshAgent>().speed;
                navMeshAgent.Resume();
            }
            else
            {
                animController.AnimateAimStanding();
                navMeshAgent.speed = navSpeedDefault;
                navMeshAgent.Stop();
            }
        }
        else
        {
            if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
                animController.AnimateMovement(animSpeed);
            else
                animController.AnimateIdle();
        }

    }

    //OTHER//

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
        targetedFriend = null;
        friendClicked = false;
        animSpeed = 0.0f;
        specialTargetedEnemy = null;
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

