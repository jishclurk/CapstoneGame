using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShoot : IAbility {

    public string name { get; set; }
    public float effectiveRange { get; set; }
    public float baseDamage { get; set; }
    public float fireRate { get; set; }
    public bool isbasicAttack { get; set; }
    public float timeToCast { get; set; }
    public float coolDownTime { get; set; }
    public float lastUsedTime { get; set; }

    public PistolShoot()
    {
        name = "Pistol Shoot";
        effectiveRange = 3.0f;
        baseDamage = 20.0f;
        fireRate = 0.5f;
        isbasicAttack = true;
        timeToCast = 0.0f;
        coolDownTime = 0.0f;
        lastUsedTime = 0.0f;

    }

    public void Execute(GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        lastUsedTime = Time.time;
        Debug.Log(name + " on " + target.name + " does " + baseDamage + " damage.");
    }

    public bool isReady()
    {
        return true; //true because this is a basic attack
    }
}
