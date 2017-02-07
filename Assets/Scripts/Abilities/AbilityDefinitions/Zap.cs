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
    public float coolDownTime { get; set; }
    public float lastUsedTime { get; set; }
    public bool requiresTarget { get; set; }
    private float nextFire;

    public Zap()
    {
        name = "Zap";
        effectiveRange = 9.0f;
        baseDamage = 50.0f;
        fireRate = 0.0f;
        isbasicAttack = false;
        timeToCast = 0.0f;
        coolDownTime = 5.0f;
        lastUsedTime = -Mathf.Infinity;
        requiresTarget = true;
    }

    public void Execute(CharacterAttributes attributes, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        lastUsedTime = Time.time;
        float adjustedDamage = baseDamage + attributes.Intelligence*2;
        Debug.Log(name + " on " + target.name + " does " + adjustedDamage + " damage.");
        target.GetComponent<EnemyHealth>().TakeDamage(adjustedDamage);
    }

    public bool isReady()
    {
        return Time.time > lastUsedTime + coolDownTime;
    }
}
