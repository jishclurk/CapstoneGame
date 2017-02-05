using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateControl : MonoBehaviour {

    public float chasingDuration = 4f;
    public Transform returnLocation;
    public float deaggroDistance = 20f;
    public MeshRenderer meshRendererFlag;

    [HideInInspector]
    public NavMeshAgent navMeshAgent;

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

    [HideInInspector]
    public List<GameObject> visiblePlayers;

    [HideInInspector]
    public GameObject chaseTarget;


    private void Awake()
    {
        chasingState = new ChasingState(this);
        idleState = new IdleState(this);
        returningState = new ReturningState(this);
        attackingState = new AttackingState(this);
        visiblePlayers = new List<GameObject>();

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

	// Use this for initialization
	private void Start () {
        currentState = idleState;
	}
	
	// Update is called once per frame
	private void FixedUpdate () {

        // Check for death here

        currentState.UpdateState();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            visiblePlayers.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            visiblePlayers.Remove(other.gameObject);
        }
    }

    public void DisableNavRotation()
    {
        navMeshAgent.updateRotation = false;
    }

    public void EnableNavRotation()
    {
        navMeshAgent.updateRotation = true;
    }
}
