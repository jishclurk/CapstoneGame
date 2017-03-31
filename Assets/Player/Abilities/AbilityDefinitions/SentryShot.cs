﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//THIS ABILITY SHOULD NOT BE ADDED AS AN UNLOCKABLE ABILITY. THIS IS ONLY A BASIC USED BY THE SENTRY ABILITY

public class SentryShot : IBasic, IAbility {

    public string name { get; set; }
    public string description { get; set; }
    public int id { get; private set; }
    public Image image { get; private set; }
    public float effectiveRange { get; set; }

    public float baseDamage { get; set; }
    public float fireRate { get; set; }
    private float lastUsedTime { get; set; }

    public int StrengthRequired { get; private set; }
    public int StaminaRequired { get; private set; }
    public int IntelligenceRequired { get; private set; }

    private Object bullet = Resources.Load("RifleShot/RifleProjectile");

    public SentryShot()
    {
        StrengthRequired = 0;
        StaminaRequired = 0;
        IntelligenceRequired = 0;
        image = Resources.Load("Abilities/Pistol", typeof(Image)) as Image;
        id = 8;
        name = "Sentry Shot";
        effectiveRange = 9.0f;
        baseDamage = 5.0f;
        fireRate = 0.15f;
        lastUsedTime = 0.0f;

    }

    public void Execute(Player player, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        lastUsedTime = Time.time;
        float adjustedDamage = baseDamage + player.attributes.TotalStrength * 0.5f;
        target.GetComponent<EnemyHealth>().TakeDamage(adjustedDamage);

        GameObject project = Object.Instantiate(bullet, origin.transform.position, Quaternion.identity) as GameObject;
        project.GetComponent<RifleProjectileScript>().destination = new Vector3(target.transform.position.x, (origin.transform.position.y), target.transform.position.z);
        project.GetComponent<RifleProjectileScript>().targetedEnemy = target;
        //useEnergy not required
    }

    public bool isReady()
    {
        return Time.time > lastUsedTime + fireRate;
    }

    public AbilityHelper.Action GetAction()
    {
        return AbilityHelper.Action.Basic;
    }

    public AbilityHelper.CoopAction GetCoopAction()
    {
        return AbilityHelper.CoopAction.Basic;
    }
}