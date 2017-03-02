using UnityEngine;
using System.Collections;

public class RifleProjectileScriptOld : MonoBehaviour
{
    public GameObject impactParticle;
    public Vector3 destination;
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
        GetComponent<Rigidbody>().velocity = normalizedTrajectory * 50.0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasCollided && other.CompareTag("Enemy") && !other.isTrigger)
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
}