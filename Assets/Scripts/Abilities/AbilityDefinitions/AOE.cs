using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AOE : ISpecial, IAbility {

    public string name { get; set; }
    public string description { get; set; }
    public float effectiveRange { get; set; }
    public float baseDamage { get; set; }
    public float energyRequired { get; set; }
    public float timeToCast { get; set; }
    public float coolDownTime { get; set; }
    private float lastUsedTime { get; set; }
    public Object aoeTarget { get; set; }
    public int id { get; private set; }
    public Image image { get; private set; }

    public AOE()
    {
        image = GameObject.Instantiate(Resources.Load("Abilities/Grenade", typeof(Image))) as Image;
        id = 0;
        name = "Area of Effect";
        effectiveRange = 10.0f;
        baseDamage = 25.0f;
        timeToCast = 0.0f;
        coolDownTime = 1.0f;
        lastUsedTime = -Mathf.Infinity;
        energyRequired = 10.0f;
        aoeTarget = Resources.Load("3x3RedAuraTarget");
    }

    public void Execute(CharacterAttributes attributes, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        lastUsedTime = Time.time;
        float adjustedDamage = baseDamage + attributes.Strength * 2;
        Debug.Log(name + " on " + target.name + " does " + adjustedDamage + " damage.");
        AOETargetController aoeController = target.GetComponent<AOETargetController>();

        foreach (GameObject enemy in aoeController.affectedEnemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(adjustedDamage);
        }
        origin.GetComponent<PlayerResources>().UseEnergy(energyRequired);
        
    }


    public bool isReady()
    {
        return Time.time > lastUsedTime + coolDownTime;
    }

    public float RemainingTime()
    {
        return lastUsedTime + coolDownTime - Time.time;
    }

    public AbilityHelper.Action GetAction()
    {
        return AbilityHelper.Action.AOE;
    }
}
