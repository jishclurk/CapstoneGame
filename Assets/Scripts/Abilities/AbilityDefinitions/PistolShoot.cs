using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShot : IAbility {

    public string name { get; set; }
    public float effectiveRange { get; set; }
    public float baseDamage { get; set; }
    public float fireRate { get; set; }
    public bool isbasicAttack { get; set; }
    public float timeToCast { get; set; }
    public float coolDownTime { get; set; }
    public float lastUsedTime { get; set; }
    public bool requiresTarget { get; set; }

    public PistolShot()
    {
        name = "Pistol Shot";
        effectiveRange = 5.0f;
        baseDamage = 20.0f;
        fireRate = 0.5f;
        isbasicAttack = true;
        timeToCast = 0.0f;
        coolDownTime = 0.0f;
        lastUsedTime = 0.0f;
        requiresTarget = true;

    }

    public void Execute(CharacterAttributes attributes, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        lastUsedTime = Time.time;
        Debug.Log(name + " on " + target.name + " does " + (baseDamage + attributes.Strength * 0.1f) + " damage.");
    }

    public bool isReady()
    {
        return Time.time > lastUsedTime + fireRate;
    }
}
