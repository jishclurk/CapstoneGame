using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControllerMachine : MonoBehaviour {

    //members needed for functionality. This could get long? Separate scripts perhaps.
    //I put these here because state transitions shouldn't take parameters.

    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public NavMeshAgent navMeshAgent;
    [HideInInspector]
    public Transform targetedEnemy;
    [HideInInspector]
    public Ray shootRay;
    [HideInInspector]
    public RaycastHit shootHit;
    [HideInInspector]
    public bool walking;
    [HideInInspector]
    public bool enemyClicked; //How does AI determine if the team is currently in combat? Should playerController sent click info to TM? But it's not just if enemyClicked, relates to # enemies too.
    [HideInInspector]
    public bool selectingAbilityTarget = false;
    [HideInInspector]
    public IAbility activeAbility;
    [HideInInspector]
    public CharacterAttributes attributes;
    [HideInInspector]
    public PlayerAbilities abilities;
    [HideInInspector]
    public TeamManager tm;


    [HideInInspector] public ICoopState currentState;
    [HideInInspector] public PlayerIdleState idleState;
    [HideInInspector] public PlayerMoveState moveState;
    [HideInInspector] public PlayerAttackState attackState;
    [HideInInspector] public PlayerCastState castState;

    private void Awake()
    {
        idleState = new PlayerIdleState(this);
        moveState = new PlayerMoveState(this);
        attackState = new PlayerAttackState(this);
        castState = new PlayerCastState(this);

        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        abilities = new PlayerAbilities();
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>() ;
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

}
