using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateControl : MonoBehaviour {

    public float chasingDuration = 4f;
    public float deaggroDistance = 20f;

    [HideInInspector]
    public Vector3 returnPosition;

    [HideInInspector]
    public NavMeshAgent navMeshAgent;

    [HideInInspector]
    public IEnemyState currentState;

    [HideInInspector]
    public ChasingState chasingState;

    [HideInInspector]
    public EnemyIdleState idleState;

    [HideInInspector]
    public ReturningState returningState;

    [HideInInspector]
    public AttackingState attackingState;

    [HideInInspector]
    public List<GameObject> visiblePlayers;

    [HideInInspector]
    public EnemyAnimationController animator;

    [HideInInspector]
    public EnemyAttack attack;

    [HideInInspector]
    public EnemySoundController sounds;

    [HideInInspector]
    public Transform eyes;

    [HideInInspector]
    public TeamManager tm;

    private EnemyHealth health;
    private GameObject chaseTarget;
    private PlayerResources chaseTargetResources;
    

    private void Awake()
    {
        chasingState = new ChasingState(this);
        idleState = new EnemyIdleState(this);
        returningState = new ReturningState(this);
        attackingState = new AttackingState(this);
        visiblePlayers = new List<GameObject>();

        returnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        eyes = transform.FindChild("Eyes");
        health = GetComponent<EnemyHealth>();
        attack = GetComponent<EnemyAttack>();
        sounds = GetComponent<EnemySoundController>();
        animator = GetComponent<EnemyAnimationController>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
    }

	// Use this for initialization
	private void Start () {
        currentState = idleState;
	}
	
	// Update is called once per frame
	private void FixedUpdate () {
        // Check for death here
        if (!health.isDead)
            currentState.UpdateState();
        else
        {
            DisableNavRotation();
            DisableNavPosition();
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            Debug.Log("Player Seen");
            visiblePlayers.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            Debug.Log("Player Left");
            visiblePlayers.Remove(other.gameObject);
        }
    }

    public void DamageCurrentTarget()
    {
        if (chaseTargetResources != null)
        {
            chaseTargetResources.TakeDamage(attack.attackDamage);
        }
    }

    public void ChangeTarget(GameObject newTarget)
    {
        chaseTarget = newTarget;
        chaseTargetResources = newTarget.GetComponent<PlayerResources>();
    }

    public Vector3 GetTargetPosition()
    {
        if (chaseTarget != null)
            return chaseTarget.transform.position;
        else
            return transform.position;
    }

    public bool IsTargetDead()
    {
        if (chaseTargetResources != null)
            return chaseTargetResources.isDead;

        return true;
    }

    public void DisableNavRotation()
    {
        navMeshAgent.updateRotation = false;
    }

    public void EnableNavRotation()
    {
        navMeshAgent.updateRotation = true;
    }

    public void DisableNavPosition()
    {
        navMeshAgent.updatePosition = false;
    }

    public void EnableNavPosition()
    {
        navMeshAgent.updatePosition = true;
    }

    

}
