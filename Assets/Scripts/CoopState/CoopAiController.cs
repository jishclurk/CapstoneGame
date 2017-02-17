using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CoopAiController : MonoBehaviour {

    //members needed for functionality. This could get long? Separate scripts perhaps.
    //I put these here because state transitions shouldn't take parameters.
    [HideInInspector]
    public Animator anim;


    [HideInInspector]
    public NavMeshAgent navMeshAgent;

    //move state variables
    [HideInInspector]
    public float followDist = 3.8f;
    [HideInInspector]
    public float followEpsilon = 2.0f; //Determines how far player has to move for ai to start to follow again

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
    public IAbility activeAbility;
    [HideInInspector]
    public Transform targetedEnemy;
    [HideInInspector]
    public Transform eyes;

    [HideInInspector]
    public PlayerAbilities abilities;
    [HideInInspector]
    public CharacterAttributes attributes;
    [HideInInspector]
    public TeamManager tm; //is this bad practice? Used to find which players are ai controlled or not

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
        animController = GetComponent<PlayerAnimationController>();
        walkSpeed = 1.0f;
        navMeshAgent = GetComponent<NavMeshAgent>();

        eyes = transform.FindChild("Eyes");
        sightCollider = GetComponent<SphereCollider>();
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>() ;

        //this is a mess. These are "shared" variables between co-op ai and player script
        Player player = GetComponent<Player>();
        abilities = player.abilities;
        activeAbility = abilities.Basic;
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
        return Physics.Raycast(eyes.position, playerToTarget, out hit) && hit.collider.gameObject.CompareTag("Enemy");
    }

    //Reports if the aiPlayer has sight on at least one enemy
    private bool CheckLocalVision()
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
        if (abilities != null)
        {
            activeAbility = abilities.Basic;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger && other.tag.Equals("Enemy"))
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
