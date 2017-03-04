using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeThrow : ISpecial, IAbility {

    public string name { get; set; }
    public string description { get; set; }
    public int id { get; private set; }
    public Image image { get; private set; }
    public float effectiveRange { get; set; }

    public float baseDamage { get; set; }
    public float energyRequired { get; set; }
    public float coolDownTime { get; set; }
    public float timeToCast { get; set; }
    public Object aoeTarget { get; set; }

    private float lastUsedTime;
    private Object explosion;
    private Object grenade;
    private GameObject abilityObj;

    public int StrengthRequired { get; private set; }
    public int StaminaRequired { get; private set; }
    public int IntelligenceRequired { get; private set; }

    public GrenadeThrow()
    {
        StrengthRequired = 3;
        StaminaRequired = 0;
        IntelligenceRequired = 0;
        image = GameObject.Instantiate(Resources.Load("Abilities/Grenade", typeof(Image))) as Image;
        id = 0;
        name = "Grenade Throw";
        effectiveRange = 10.0f;
        baseDamage = 25.0f;
        timeToCast = 1.0f;
        coolDownTime = 5.0f;
        lastUsedTime = -Mathf.Infinity;
        energyRequired = 25.0f;
        aoeTarget = Resources.Load("GrenadeThrow/3x3RedAuraTarget");
        explosion = Resources.Load("GrenadeThrow/explosion");
        abilityObj = GameObject.FindWithTag("AbilityHelper");
        grenade = Resources.Load("GrenadeThrow/nade");
    }

    public void Execute(Player player, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {  
        abilityObj.GetComponent<AbilityHelper>().GrenadeThrowRoutine(player.attributes, origin, target, baseDamage, explosion, timeToCast, grenade);
        player.animController.AnimateUse(0.35f);
        /*float adjustedDamage = baseDamage + attributes.Strength * 2;
        Debug.Log(name + " on " + target.name + " does " + adjustedDamage + " damage.");
        AOETargetController aoeController = target.GetComponent<AOETargetController>();

        foreach (GameObject enemy in aoeController.affectedEnemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(adjustedDamage);
        }
        
        Object exp = Object.Instantiate(explosion, target.transform.position, Quaternion.identity);
        Object.Destroy(exp, 1.0f);*/

        lastUsedTime = Time.time;
        
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

    public AbilityHelper.CoopAction GetCoopAction()
    {
        return AbilityHelper.CoopAction.AOEHurt;
    }
}
