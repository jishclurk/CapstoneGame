using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LayerDefinitions;

public class SentryScript : MonoBehaviour {

    public Player castedPlayer;
    private IBasic sentryAbility;
    private Transform[] gunfx;
    private Transform pylon;
    private HashSet<GameObject> watchedEnemies;
    private HashSet<GameObject> visibleEnemies;

    private Transform targetedEnemy;
    private TeamManager tm;

    private int gunToFire = 0;

	// Use this for initialization
	void Start () {
		if(castedPlayer == null)
        {
            Debug.Log("SENTRY TURRET DOES NOT HAVE CASTED PLAYER");
            Destroy(gameObject);
        }
        pylon = transform.FindDeepChild("Turret");
        gunfx = new Transform[3];
        gunfx[0] = pylon.FindChild("Gun0");
        gunfx[1] = pylon.FindChild("Gun1");
        gunfx[2] = pylon.FindChild("Gun2");
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
        sentryAbility = new SentryShot();
        watchedEnemies = new HashSet<GameObject>();
        visibleEnemies = new HashSet<GameObject>();
        StartCoroutine(addCurrentEnemiesToWatched());
        
    }
	
    private IEnumerator addCurrentEnemiesToWatched()
    {
        yield return new WaitForEndOfFrame();
        GetComponent<Collider>().enabled = false;
        yield return new WaitForEndOfFrame();
        GetComponent<Collider>().enabled = true;
    }

	// Update is called once per frame
	void Update () {
        CheckLocalVision();
        FindLowestHealthTarget();
        if(targetedEnemy != null)
        {
            ShootGun();
        }

	}

    private void ShootGun()
    {
        if (targetedEnemy == null)
        {
            return;
        }
        float remainingDistance = Vector3.Distance(targetedEnemy.position, transform.position);
        if (remainingDistance <= sentryAbility.effectiveRange && isTargetVisible(targetedEnemy))
        {
            //Within range, look at enemy and shoot
            pylon.LookAt(new Vector3(targetedEnemy.position.x, pylon.position.y, targetedEnemy.position.z));
            pylon.Rotate(0, -90, 0);

            if (sentryAbility.isReady() && !targetedEnemy.GetComponent<EnemyHealth>().isDead)
            {
                sentryAbility.Execute(castedPlayer, gunfx[gunToFire % 3].gameObject, targetedEnemy.gameObject);
                gunToFire++;
            } else if (targetedEnemy.GetComponent<EnemyHealth>().isDead)
            {
                visibleEnemies.Remove(targetedEnemy.gameObject);
                watchedEnemies.Remove(targetedEnemy.gameObject);
            }
        }
    }

    public bool isTargetVisible(Transform target)
    {
        RaycastHit hit;
        Vector3 playerToTarget = target.position - gunfx[0].position;
        return Physics.Raycast(gunfx[0].position, playerToTarget, out hit, 100f, Layers.NonPlayer) && hit.collider.gameObject.CompareTag("Enemy");
    }

    public bool CheckLocalVision()
    {
        bool canSeeOneEnemy = false;
        foreach (GameObject enemy in watchedEnemies)
        {
            if (enemy != null)
            {
                if (isTargetVisible(enemy.transform) && !enemy.GetComponent<EnemyHealth>().isDead)
                {
                    visibleEnemies.Add(enemy);
                    tm.visibleEnemies.Add(enemy);
                    canSeeOneEnemy = true;
                }
            }
        }
        return canSeeOneEnemy;
    }

    private void FindLowestHealthTarget()
    {
        float lowestHealth = float.PositiveInfinity;
        GameObject bestTarget = null;

        foreach (GameObject enemy in visibleEnemies)
        {
            if (enemy != null)
            {
                float enemyHealth = enemy.GetComponent<EnemyHealth>().currentHealth;
                if (enemyHealth < lowestHealth)
                {
                    lowestHealth = enemyHealth;
                    bestTarget = enemy;
                }
            }
        }

        if (bestTarget != null)
        {
            targetedEnemy = bestTarget.transform;
        }

    }

    //update shared watchedEnemies between co-op and ai
    private void OnTriggerEnter(Collider other)
    {

        if (!other.isTrigger && other.tag.Equals("Enemy") && !other.GetComponent<EnemyHealth>().isDead)
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
        }
    }
}
