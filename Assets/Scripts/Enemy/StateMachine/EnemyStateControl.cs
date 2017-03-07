using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateControl : MonoBehaviour {

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
    public EnemyAnimationController animator;

    [HideInInspector]
    public EnemyAttack attack;

    [HideInInspector]
    public EnemySoundController sounds;

    [HideInInspector]
    public Transform eyes;

    [HideInInspector]
    public TeamManager tm;

    private EnemyMobKnowledge mobKnowledge;
    private EnemyHealth health;
    private GameObject chaseTarget;
    private PlayerResources chaseTargetResources;
    private List<GameObject> visiblePlayers;
    private bool isTargetting;


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

        isTargetting = false;

        if (transform.parent.CompareTag("EnemyMob"))
            mobKnowledge = GetComponentInParent<EnemyMobKnowledge>();

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
            StopTargetting();
            foreach (GameObject player in visiblePlayers)
                mobKnowledge.RemoveVisiblePlayer(player);

            navMeshAgent.enabled = false;
            this.enabled = false;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            visiblePlayers.Add(other.gameObject);
            mobKnowledge.AddVisiblePlayer(other.gameObject);
            FindTarget();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            visiblePlayers.Remove(other.gameObject);
            mobKnowledge.RemoveVisiblePlayer(other.gameObject);
        }
    }

    public void DamageCurrentTarget()
    {
        if (chaseTargetResources != null)
        {
            chaseTargetResources.TakeDamage(attack.attackDamage);
        }
    }

    public void FindTarget()
    {
        if (isTargetting)
            StopTargetting();

        chaseTarget = mobKnowledge.GetNewTarget();
        if (chaseTarget != null)
        {
            chaseTargetResources = chaseTarget.GetComponent<PlayerResources>();
            isTargetting = true;
        }
    }

    public void StopTargetting()
    {
        if (chaseTarget != null && isTargetting)
        {
            mobKnowledge.RemoveTargettedPlayer(chaseTarget);
            isTargetting = false;
        }  
    }

    public void ReportTargetOutOfRange()
    {
        mobKnowledge.RemoveKnowledgeOfPlayer(chaseTarget);
    }

    public List<GameObject> GetVisiblePlayers()
    {
        if (mobKnowledge != null)
            return mobKnowledge.GetVisiblePlayers();

        return visiblePlayers;
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
        {
            if (chaseTargetResources.isDead)
                mobKnowledge.RemoveKnowledgeOfPlayer(chaseTarget);
            return chaseTargetResources.isDead;
        }

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

}
