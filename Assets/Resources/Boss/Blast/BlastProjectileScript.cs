using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastProjectileScript : MonoBehaviour {

    public GameObject impactParticle;
    public Vector3 destination;
    public float damage;
    public GameObject projectileParticle;
    [HideInInspector]
    public Vector3 impactNormal; //Used to rotate impactparticle.
    public float rot;
    public float velocityMultiplier = 10.0f;


    void Start()
    {
        transform.LookAt(destination);
        transform.Rotate(0.0f, rot, 0.0f);
        projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        projectileParticle.transform.parent = transform;
        Vector3 normalizedTrajectory = transform.forward;
        GetComponent<Rigidbody>().velocity = normalizedTrajectory * velocityMultiplier;
        Destroy(gameObject, 7f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && !other.GetComponent<PlayerResources>().isDead)
        {
            other.GetComponent<PlayerResources>().TakeDamage(damage);
            impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
            Destroy(impactParticle, 5f);
        }
    }
}
