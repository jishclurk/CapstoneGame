using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateControl : MonoBehaviour {

    public float chasingDuration = 4f;
    public float sightRange = 20f;
    public Transform returnLocation;
    public MeshRenderer meshRendererFlag;

    [HideInInspector]
    public NavMeshAgent navMeshAgent;

    [HideInInspector]
    public Transform chaseTarget;

    [HideInInspector]
    public IEnemyState currentState;

    [HideInInspector]
    public ChasingState chasingState;

    [HideInInspector]
    public IdleState idleState;

    [HideInInspector]
    public ReturningState returningState;

    [HideInInspector]
    public AttackingState attackingState;
    

    private void Awake()
    {
        chasingState = new ChasingState(this);
        idleState = new IdleState(this);
        returningState = new ReturningState(this);
        attackingState = new AttackingState(this);

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

	// Use this for initialization
	private void Start () {
        currentState = idleState;
	}
	
	// Update is called once per frame
	private void FixedUpdate () {
        currentState.UpdateState();
	}

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }
}
