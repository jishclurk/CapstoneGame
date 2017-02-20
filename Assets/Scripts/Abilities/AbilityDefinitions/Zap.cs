using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zap : IAbility {

    public string name { get; set; }
    public float effectiveRange { get; set; }
    public float baseDamage { get; set; }
    public float fireRate { get; set; }
    public bool isbasicAttack { get; set; }
    public float timeToCast { get; set; }
    public float energyRequired { get; set; }
    public float coolDownTime { get; set; }
    public float lastUsedTime { get; set; }
    public bool requiresTarget { get; set; }
    public bool requiresAim { get; set; }
    public Object aoeTarget { get; set; }
    private float nextFire;

    public Zap()
    {
        name = "Zap";
        effectiveRange = 9.0f;
        baseDamage = 40.0f;
        fireRate = 0.0f;
        isbasicAttack = false;
        timeToCast = 0.0f;
        coolDownTime = 5.0f;
        lastUsedTime = -Mathf.Infinity;
        requiresTarget = true;
        energyRequired = 30.0f;
        requiresAim = false;
        aoeTarget = null;
    }

    public void Execute(CharacterAttributes attributes, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        lastUsedTime = Time.time;
        float adjustedDamage = baseDamage + attributes.Intelligence*2;
        Debug.Log(name + " on " + target.name + " does " + adjustedDamage + " damage.");
        target.GetComponent<EnemyHealth>().TakeDamage(adjustedDamage); //At some point we may want to look at tag of origin/targe to access appropriate scripts
        origin.GetComponent<PlayerResources>().UseEnergy(energyRequired);
    }

    public bool isReady()
    {
        return Time.time > lastUsedTime + coolDownTime;
    }
}
