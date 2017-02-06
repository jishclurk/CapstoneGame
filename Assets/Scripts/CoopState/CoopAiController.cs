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
    [HideInInspector]
    public float followDist = 3.8f;
    [HideInInspector]
    public float followEpsilon = 2.0f; //Determines how far player has to move for ai to start to follow again

    [HideInInspector]
    public PlayerAbilities abilities;
    [HideInInspector]
    public CharacterAttributes attributes;
    [HideInInspector]
    public TeamManager tm; //is this bad practice? Used to find which players are ai controlled or not


    [HideInInspector] public ICoopState currentState;
    [HideInInspector] public IdleState idleState;
    [HideInInspector] public MoveState moveState;
    [HideInInspector] public AttackState attackState;
    [HideInInspector] public CastState castState;

    private void Awake()
    {
        idleState = new IdleState(this);
        moveState = new MoveState(this);
        attackState = new AttackState(this);
        castState = new CastState(this);

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
