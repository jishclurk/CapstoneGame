using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyAbility : IAbility {

    public string name { get; set; }
    public float effectiveRange { get; set; }
    public float baseDamage { get; set; }
    public float fireRate { get; set; }
    public bool isbasicAttack { get; set; }
    public float timeToCast { get; set; }
    public float coolDownTime { get; set; }
    public float lastUsedTime { get; set; }
    public bool requiresTarget { get; set; }

    public EmptyAbility()
    {
        name = "Empty Ability";
        effectiveRange = 0.0f;
        baseDamage = 0.0f;
        fireRate = 0.0f;
        isbasicAttack = true;
        timeToCast = 0.0f;
        coolDownTime = 0.0f;
        lastUsedTime = 0.0f;
        requiresTarget = true;

    }

    public void Execute(CharacterAttributes attributes, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {

    }

    public bool isReady()
    {
        return false;
    }
}
