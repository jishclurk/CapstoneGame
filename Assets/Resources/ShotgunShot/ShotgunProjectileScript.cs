﻿using UnityEngine;
using System.Collections;

public class ShotgunProjectileScript : MonoBehaviour
{
    public GameObject impactParticle;
    public Vector3 destination;
    public float damage;
    public GameObject projectileParticle;
    public GameObject[] trailParticles;
    [HideInInspector]
    public Vector3 impactNormal; //Used to rotate impactparticle.
    public float rot;

    private bool hasCollided = false;

    void Start()
    {
        
        transform.LookAt(destination);
        transform.Rotate(0.0f, rot, 0.0f);
        projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        projectileParticle.transform.parent = transform;
        Vector3 normalizedTrajectory = transform.forward;
        GetComponent<Rigidbody>().velocity = normalizedTrajectory * 30.0f;
        Destroy(gameObject, 0.5f);
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
        if (!hasCollided && other.CompareTag("Enemy") && !other.isTrigger && !other.GetComponent<EnemyHealth>().isDead)
        {
            hasCollided = true;
            //transform.DetachChildren();
            impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
            //Debug.DrawRay(hit.contacts[0].point, hit.contacts[0].normal * 1, Color.yellow);
            other.GetComponent<EnemyHealth>().TakeDamage(damage);

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