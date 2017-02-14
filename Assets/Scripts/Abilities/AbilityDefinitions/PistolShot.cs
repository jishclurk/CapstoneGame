﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShot : IAbility {

    public string name { get; set; }
    public float effectiveRange { get; set; }
    public float baseDamage { get; set; }
    public float fireRate { get; set; }
    public bool isbasicAttack { get; set; }
    public float energyRequired { get; set; }
    public float timeToCast { get; set; }
    public float coolDownTime { get; set; }
    public float lastUsedTime { get; set; }
    public bool requiresTarget { get; set; }

    public PistolShot()
    {
        name = "Pistol Shot";
        effectiveRange = 9.0f;
        baseDamage = 5.0f;
        fireRate = 0.8f;
        isbasicAttack = true;
        timeToCast = 0.0f;
        coolDownTime = 0.0f;
        lastUsedTime = 0.0f;
        requiresTarget = true;
        energyRequired = 0.0f;

    }

    public void Execute(CharacterAttributes attributes, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        lastUsedTime = Time.time;
        float adjustedDamage = baseDamage + attributes.Strength * 0.1f;
        Debug.Log(name + " on " + target.name + " does " + adjustedDamage + " damage.");
        target.GetComponent<EnemyHealth>().TakeDamage(adjustedDamage);
        //useEnergy not required
    }

    public bool isReady()
    {
        return Time.time > lastUsedTime + fireRate;
    }
}