using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zap : IAbility {

    public float effectiveRange { get; set; }
    public float baseDamage { get; set; }
    public float fireRate { get; set; }
    public bool repeating { get; set; }

    public Zap()
    {
        effectiveRange = 5.0f;
        baseDamage = 100.0f;
        fireRate = 0.5f;
        repeating = false;
    }

    public void Execute(GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        Debug.Log("Zap on " + target.name + " does " + baseDamage + " damage.");
    }
}
