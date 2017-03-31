using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainProjectileScript : MonoBehaviour {

    public GameObject impactParticle;
    public Vector3 destination;
    public float damage;
    public HashSet<GameObject> enemyPool;
    public GameObject targetedEnemy;
    public GameObject projectileParticle;
    public GameObject[] trailParticles;
    [HideInInspector]
    public Vector3 impactNormal; //Used to rotate impactparticle.

    private bool hasCollided = false;

    void Start()
    {
        transform.LookAt(destination);
        projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        projectileParticle.transform.parent = transform;
        Vector3 normalizedTrajectory = Vector3.Normalize(destination - transform.position);
        GetComponent<Rigidbody>().velocity = normalizedTrajectory * 35.0f;
        Destroy(gameObject, 2.0f);
    }

    /*
    void Update()
    {

        if(!hasCollided && Vector3.Distance(destination, transform.position) < 0.2f)
        {
            hasCollided = true;
            //transform.DetachChildren();
            impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
            //Debug.DrawRay(hit.contacts[0].point, hit.contacts[0].normal * 1, Color.yellow);

            //yield WaitForSeconds (0.05);
            foreach (GameObject trail in trailParticles)
            {
                GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
                curTrail.transform.parent = null;
                Destroy(curTrail, 3f);
            }
            Destroy(projectileParticle, 3f);
            Destroy(impactParticle, 5f);
            Destroy(gameObject);
            //projectileParticle.Stop();
        }
    }
    */

    void OnTriggerEnter(Collider other)
    {
        if (!hasCollided && other.gameObject == targetedEnemy && !other.isTrigger)
        {
            hasCollided = true;
            //transform.DetachChildren();
            impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
            //Debug.DrawRay(hit.contacts[0].point, hit.contacts[0].normal * 1, Color.yellow);

            //enemy takes damage
            other.GetComponent<EnemyHealth>().TakeDamage(damage);
            enemyPool.Remove(other.gameObject);

            //new gameobject is born if damage hasn't been decreased too far
            GameObject newTarget = null;
            if (damage > 10)
            {

                foreach (GameObject enemy in enemyPool)
                {
                    if (Vector3.Distance(enemy.transform.position, transform.position) < 4.0f)
                    {
                        newTarget = enemy;
                    }

                }
                if(newTarget != null)
                {
                    GameObject project = Object.Instantiate(gameObject, transform.position, Quaternion.identity) as GameObject;
                    project.GetComponent<ChainProjectileScript>().destination = new Vector3(newTarget.transform.position.x, transform.position.y, newTarget.transform.position.z);
                    project.GetComponent<ChainProjectileScript>().targetedEnemy = newTarget;
                    project.GetComponent<ChainProjectileScript>().enemyPool = enemyPool;
                    project.GetComponent<ChainProjectileScript>().damage = damage * 0.8f;
                }

            }

            //yield WaitForSeconds (0.05);
            foreach (GameObject trail in trailParticles)
            {
                GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
                curTrail.transform.parent = null;
                Destroy(curTrail, 3f);
            }
            Destroy(projectileParticle, 3f);
            Destroy(impactParticle, 5f);
            Destroy(gameObject);
            //projectileParticle.Stop();
        }
    }
}
