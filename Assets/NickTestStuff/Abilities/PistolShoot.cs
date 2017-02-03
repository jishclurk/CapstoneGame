using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShoot : IAbility {

    public float effectiveRange { get; set; }
    public float baseDamage { get; set; }
    public float fireRate { get; set; }
    public bool repeating { get; set; }

    public PistolShoot()
    {
        effectiveRange = 3.0f;
        baseDamage = 20.0f;
        fireRate = 0.5f;
        repeating = true;
    }

    public void Execute(GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        Debug.Log("Pistol Shoot on " + target.name + " does " + baseDamage + " damage.");
    }
}
