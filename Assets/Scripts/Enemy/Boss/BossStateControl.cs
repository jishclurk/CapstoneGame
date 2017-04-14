using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateControl : MonoBehaviour {

    public float minIdleTime = 2f;
    public float maxIdleTime = 4f;
    public float rotationSpeed = 3f;
    public float timeSpentTurning = 5f;

    [HideInInspector]
    public Player currentTarget;
    private int previousTargetIndex = -1;
    private int previousAttackChoice = -1;

    private float turningTimer = 0f;
    private float timeToNextAttack = 0f;
    private bool isAggroed = false;
    private bool allPlayersDead = false;
    private BossAnimationController animator;
    private TeamManager tm;
    private EnemySoundController sound;

    private enum AttackBeingPerformed { None, Sweep, Laser, Explode, Spawn }
    private AttackBeingPerformed currentAttack = AttackBeingPerformed.None;

    // Use this for initialization
    void Awake () {
        animator = GetComponent<BossAnimationController>();
        sound = GetComponent<EnemySoundController>();
        tm = GameObject.FindGameObjectWithTag("TeamManager").GetComponent<TeamManager>();
        turningTimer = timeSpentTurning;

        animator.AnimateIdle();
    }
	
    void OnTriggerEnter(Collider other)
    {
        if (!isAggroed && other.CompareTag("Player") && !other.isTrigger && !other.GetComponent<PlayerResources>().isDead)
        {
            isAggroed = true;
            sound.PlayAggroSound();
        }
    }

	// Update is called once per frame
	void FixedUpdate ()
    {

		if (isAggroed && !allPlayersDead)
        {
            if (timeToNextAttack > 0)
            {
                timeToNextAttack -= Time.deltaTime;
                return;
            }
            else if (currentAttack == AttackBeingPerformed.Laser)
            {
                Vector3 targetDir = currentTarget.transform.position - transform.position;
                float step = rotationSpeed * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);

                transform.rotation = Quaternion.LookRotation(newDir);
            }
            else if (currentAttack == AttackBeingPerformed.None)
            {
                if (currentTarget == null)
                    currentTarget = SelectTarget();

                turningTimer -= Time.deltaTime;
                if (turningTimer <= 0)
                {
                    turningTimer = timeSpentTurning;
                    PerformAttack();
                    return;
                }
                else if (allPlayersDead)
                    return;

                Vector3 targetDir = currentTarget.transform.position - transform.position;
                float step = rotationSpeed * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
                if (Vector3.Angle(newDir, targetDir) < 5)
                    PerformAttack();
                else if (Vector3.Cross(newDir, targetDir).y < 0)
                    animator.AnimateLeftTurn();
                else
                    animator.AnimateRightTurn();

                transform.rotation = Quaternion.LookRotation(newDir);
            }
        }
	}


    public void ReturnToIdle()
    {
        timeToNextAttack = Random.Range(minIdleTime, maxIdleTime);
        previousTargetIndex = currentTarget.id;
        currentAttack = AttackBeingPerformed.None;
        currentTarget = null;
        animator.AnimateIdle();
    }

    private Player SelectTarget()
    {
        List<Player> alivePlayers = new List<Player>();
        allPlayersDead = true;

        // Favor nearby targets, check for dead players
        for (int i = 0; i < tm.playerList.Count; i++)
        {
            Player player = tm.playerList[i];
            if (!player.GetComponent<PlayerResources>().isDead)
            {
                allPlayersDead = false;
                alivePlayers.Add(player);
                if (Vector3.Distance(player.transform.position, this.transform.position) < 5f && player.id != previousTargetIndex && Random.value > 0.7)
                    return player;
            }
        }

        if (allPlayersDead)
        {
            animator.AnimateIdle();
            return null;
        }
            
        int choice = Random.Range(0, alivePlayers.Count);

        // Chance to reroll target if its the same as the last target
        if (alivePlayers[choice].id == previousTargetIndex)
            choice = Random.Range(0, alivePlayers.Count);

        return alivePlayers[choice];
    }

    private void PerformAttack()
    {
        foreach (Player player in tm.playerList)
        {
            if (Vector3.Distance(player.transform.position, this.transform.position) < 3f && Random.value > 0.8)
            {
                currentAttack = AttackBeingPerformed.Explode;
                animator.AnimateExplodeAttack();
                return;
            }
        }

        int choice = Random.Range(0, 4);
        if (choice == previousAttackChoice)
            choice = Random.Range(0, 4);
        previousAttackChoice = choice;

        switch (choice)
        {
            case 0:
                currentAttack = AttackBeingPerformed.Sweep;
                animator.AnimateSweepAttack();
                break;
            case 1:
                currentAttack = AttackBeingPerformed.Laser;
                animator.AnimateLaserAttack();
                break;
            case 2:
                currentAttack = AttackBeingPerformed.Explode;
                animator.AnimateExplodeAttack();
                break;
            case 3:
                currentAttack = AttackBeingPerformed.Spawn;
                animator.AnimateSpawnAttack();
                break;
            default:
                currentAttack = AttackBeingPerformed.Spawn;
                animator.AnimateSpawnAttack();
                break;
        }
    }
}
