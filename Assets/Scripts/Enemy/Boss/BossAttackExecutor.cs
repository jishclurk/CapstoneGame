using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackExecutor : MonoBehaviour {

    public Transform SweepProjectileOrigin;
    public float SweepProjectileDamage = 15f;
    public float ExplosionRadius = 6f;
    public float ExplosionDamage = 15f;

    public Transform tempTarget;
    private Object sweepProj;
    private Object explostionEffect;
    private TeamManager tm;


    // Use this for initialization
    void Start ()
    {
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
        sweepProj = Resources.Load("Boss/Sweep/SweepProjectile");
        explostionEffect = Resources.Load("Boss/Explosion/ExplosionEffect");
    }
	
	
    public void ExecuteSweepAttack()
    {
        Vector3 targetDest = new Vector3(tempTarget.transform.position.x, SweepProjectileOrigin.position.y, tempTarget.transform.position.z);

        for (float shellRot = -30.0f; shellRot < 30.1f; shellRot += 20.0f)
        {
            GameObject project = Instantiate(sweepProj, SweepProjectileOrigin.position, Quaternion.identity) as GameObject;
            project.GetComponent<SweepProjectileScript>().destination = targetDest;
            project.GetComponent<SweepProjectileScript>().rot = shellRot;
            project.GetComponent<SweepProjectileScript>().damage = SweepProjectileDamage;
        }
    }

    public void ExecuteExplosionAttack()
    {
        GameObject explosion = Instantiate(explostionEffect, 
                                           new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z), 
                                           Quaternion.Euler(90, 0, 0)) as GameObject;

        foreach (Player player in tm.playerList) 
        {
            PlayerResources resources = player.GetComponent<PlayerResources>();
            if ((Mathf.Abs(Vector3.Distance(this.transform.position, player.transform.position)) < ExplosionRadius) && !resources.isDead)
            {
                resources.TakeDamage(ExplosionDamage);
            }
        }
    }

    public void StartLaserAttack()
    {

    }

    public void EndLaserAttack()
    {

    }

    public void ExecuteSpawnAttack()
    {

    }
}
