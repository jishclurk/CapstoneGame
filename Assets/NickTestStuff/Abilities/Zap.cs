﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zap : IAbility {

    public string name { get; set; }
    public float effectiveRange { get; set; }
    public float baseDamage { get; set; }
    public float fireRate { get; set; }
    public bool isbasicAttack { get; set; }
    public float timeToCast { get; set; }
    public float coolDownTime { get; set; }
    public float lastUsedTime { get; set; }

    public Zap()
    {
        name = "Zap";
        effectiveRange = 5.0f;
        baseDamage = 100.0f;
        fireRate = 0.5f;
        isbasicAttack = false;
        timeToCast = 0.0f;
        coolDownTime = 5.0f;
        lastUsedTime = -Mathf.Infinity;
    }

    public void Execute(GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        Debug.Log(name + " on " + target.name + " does " + baseDamage + " damage.");
    }

    public bool isReady()
    {
        return true;
    }
}
