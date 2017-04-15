﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackExecutor : MonoBehaviour {

    public Transform SweepProjectileOrigin;
    public Transform forwardTarget;
    public Transform LaserOrigin;
    public float SweepProjectileDamage = 15f;
    public float ExplosionRadius = 6f;
    public float ExplosionDamage = 15f;
    public float LaserDamage = 7f;
    public float LaserSecondsBetweenDamage = 0.7f;
    public float SpawnDamage = 10f;
    public float SpawnSecondsBetweenDamage = 1.0f;
    public float SpawnFieldDuration = 10f;
    public float BlastProjectileDamage = 30f;

    
    private Object sweepProj;
    private Object explostionEffect;
    private Object laserEffect;
    private Object spawnEffect;
    private Object blastEffect;
    private TeamManager tm;
    private BossStateControl state;

    private GameObject laserInstantiation;

    // Use this for initialization
    void Start ()
    {
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
        sweepProj = Resources.Load("Boss/Sweep/SweepProjectile");
        explostionEffect = Resources.Load("Boss/Explosion/ExplosionEffect");
        laserEffect = Resources.Load("Boss/Laser/LaserEffect");
        spawnEffect = Resources.Load("Boss/Spawn/SpawnEffect");
        blastEffect = Resources.Load("Boss/Blast/BlastProjectile");
        state = GetComponent<BossStateControl>();
    }
	
	
    public void ExecuteSweepAttack()
    {
        Vector3 targetDest = new Vector3(forwardTarget.position.x, SweepProjectileOrigin.position.y, forwardTarget.position.z);

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
        //state.rotationSpeed = state.rotationSpeed / 2;
        laserInstantiation = Instantiate(laserEffect, LaserOrigin.position, transform.rotation) as GameObject;
        laserInstantiation.transform.SetParent(this.transform);
        laserInstantiation.transform.Rotate(new Vector3(180, 0, 0));
        laserInstantiation.GetComponent<LaserScript>().damage = LaserDamage;
        laserInstantiation.GetComponent<LaserScript>().secondsBetweenDamage = LaserSecondsBetweenDamage;
    }

    public void EndLaserAttack()
    {
        //state.rotationSpeed = state.rotationSpeed * 2;
        foreach (ParticleSystem ps in laserInstantiation.GetComponentsInChildren<ParticleSystem>())
        {
            ps.Stop();
        }
        Destroy(laserInstantiation, 0.5f);
    }

    public void ExecuteSpawnAttack()
    {
        Vector3 targetDest = new Vector3(state.currentTarget.transform.position.x, state.currentTarget.transform.position.y + 1f, state.currentTarget.transform.position.z);
        GameObject spawnField = Instantiate(spawnEffect, targetDest, Quaternion.Euler(180, 0, 0)) as GameObject;
        spawnField.GetComponent<SpawnFieldScript>().damage = SpawnDamage;
        spawnField.GetComponent<SpawnFieldScript>().secondsBetweenDamage = SpawnSecondsBetweenDamage;
        Destroy(spawnField, SpawnFieldDuration);
    }

    public void ExecuteBlastAttack()
    {
        Vector3 targetDest = new Vector3(forwardTarget.position.x, SweepProjectileOrigin.position.y, forwardTarget.position.z);

        GameObject project = Instantiate(blastEffect, SweepProjectileOrigin.position, Quaternion.identity) as GameObject;
        project.GetComponent<BlastProjectileScript>().destination = targetDest;
        project.GetComponent<BlastProjectileScript>().rot = transform.rotation.y;
        project.GetComponent<BlastProjectileScript>().damage = BlastProjectileDamage;

    }
}
