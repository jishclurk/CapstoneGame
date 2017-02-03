using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{

    public float shootDistance = 4.0f;


    private Animator anim;
    private NavMeshAgent navMeshAgent;
    private Transform targetedEnemy;
    private Ray shootRay;
    private RaycastHit shootHit;
    private bool walking;
    private bool enemyClicked;
    private float nextFire;
    private List<IAbility> hotAbilities; //List? Map? Maybe a Map of hotbar key -> ability?
    private IAbility activeAbility;


    // Use this for initialization
    void Awake()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        hotAbilities = new List<IAbility>();
        hotAbilities.Add(new PistolShoot()); //This would most likely be an external function that loads the player's abilities.
        activeAbility = hotAbilities[0];
        shootDistance = activeAbility.effectiveRange;
    }


    // Update is called once per frame
    void Update()
    {


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetButtonDown("Fire2"))
        {
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    targetedEnemy = hit.transform;
                    enemyClicked = true;
                }
                else
                {
                    walking = true;
                    enemyClicked = false;
                    navMeshAgent.destination = hit.point;
                    transform.LookAt(hit.point); //prevents slow turn, could be omitted or replaced with animation
                    navMeshAgent.Resume();
                }
            }
        }


        if (enemyClicked)
        {
            MoveAndShoot();
        }
        else
        {
            walking = navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance;
        }

        anim.SetBool("Idling", !walking);
        anim.SetBool("NonCombat", !enemyClicked);
    }

    private void MoveAndShoot()
    {

        if (targetedEnemy == null)
        {
            //this return happens if enemy dies
            return; //avoid running code we don't need to.
        }
        navMeshAgent.destination = targetedEnemy.position;
        if (navMeshAgent.remainingDistance >= shootDistance)
        {
            navMeshAgent.Resume();
            walking = true;
        }
        else
        {
            //Within range, look at enemy and shoot
            transform.LookAt(targetedEnemy);
            Vector3 dirToShoot = targetedEnemy.transform.position - transform.position;
            if (activeAbility.repeating && Time.time > nextFire)
            {
                nextFire = Time.time + activeAbility.fireRate;
                activeAbility.Execute(gameObject, targetedEnemy.gameObject);

            }

            navMeshAgent.Stop(); //within range, stop moving
            walking = false;
        }


    }
}

