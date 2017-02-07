﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOE : IAbility {

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
    private float nextFire;

    public AOE()
    {
        name = "Area of Effect";
        effectiveRange = 9.0f;
        baseDamage = 100.0f;
        fireRate = 0.0f;
        isbasicAttack = false;
        timeToCast = 0.0f;
        coolDownTime = 5.0f;
        lastUsedTime = -Mathf.Infinity;
        requiresTarget = true;
        energyRequired = 20.0f;
    }

    public void Execute(CharacterAttributes attributes, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        lastUsedTime = Time.time;
        Debug.Log(name + " on " + target.name + " does " + baseDamage + " damage.");
    }

    public bool isReady()
    {
        return Time.time > lastUsedTime + coolDownTime;
    }
}
