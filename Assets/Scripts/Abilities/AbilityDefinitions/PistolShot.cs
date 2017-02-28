using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PistolShot : IBasic, IAbility {

    public string name { get; set; }
    public string description { get; set; }
    public int id { get; private set; }
    public Image image { get; private set; }
    public float effectiveRange { get; set; }

    public float baseDamage { get; set; }
    public float fireRate { get; set; }
    private float lastUsedTime { get; set; }



    public PistolShot()
    {
        image = GameObject.Instantiate(Resources.Load("Abilities/Pistol", typeof(Image))) as Image;
        id = 2;
        name = "Pistol Shot";
        effectiveRange = 9.0f;
        baseDamage = 5.0f;
        fireRate = 0.8f;
        lastUsedTime = 0.0f;

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

    public AbilityHelper.Action GetAction()
    {
        return AbilityHelper.Action.Basic;
    }
}
