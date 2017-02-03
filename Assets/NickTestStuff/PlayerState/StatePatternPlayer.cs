using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePatternPlayer : MonoBehaviour {

    //members needed for functionality. This could get long? Separate scripts perhaps.
    //I put these here because state transitions shouldn't take parameters.
    [HideInInspector] public Vector3 moveDestination;
    [HideInInspector] public Vector3 attackTargetPosition;
    [HideInInspector] public IAbility selectedAbility;

    [HideInInspector] public IPlayerState currentState;
    [HideInInspector] public IdleState idleState;
    [HideInInspector] public MoveState moveState;
    [HideInInspector] public AttackState attackState;
    [HideInInspector] public CastState castState;
    [HideInInspector] public UnityEngine.AI.NavMeshAgent navMeshAgent;

    private IStrategy stategy;

    private void Awake()
    {
        idleState = new IdleState(this);
        moveState = new MoveState(this);
        attackState = new AttackState(this);
        castState = new CastState(this);
        

        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
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


    //---- Methods that are called by multiple states ----
    public void HandleInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name == "Floor")
                {
                    Debug.Log("This is floor, MOVING!");
                    Debug.Log(hit.point);
                    moveDestination = hit.point;
                    currentState.ToMoveState();
                }
                else if (hit.transform.tag == "Enemy")
                {
                    Debug.Log("This is Enemy, ATTACKING!");
                    Debug.Log(hit.transform.gameObject);
                    currentState.ToAttackState();
                }
                else
                {
                    Debug.Log("This isn't something, IDLEING");
                    currentState.ToIdleState();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q Ability Activated!");
            //currentState.ToCastState();
        }
    }

}
