using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelfHeal : ISpecial, IAbility
{

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

    private float lastUsedTime { get; set; }




    public int StrengthRequired { get; private set; }
    public int StaminaRequired { get; private set; }
    public int IntelligenceRequired { get; private set; }

    public SelfHeal()
    {
        StrengthRequired = 0;
        StaminaRequired = 0;
        IntelligenceRequired = 3;
        image = GameObject.Instantiate(Resources.Load("Abilities/Heal", typeof(Image))) as Image;
        id = 3;
        name = "Heal";
        effectiveRange = 5.0f;
        baseDamage = 20.0f;
        timeToCast = 0.0f;
        coolDownTime = 4.0f;
        lastUsedTime = -Mathf.Infinity;
        energyRequired = 20.0f;
        aoeTarget = null;
        description = "Heal up";
    }

    public void Execute(Player player, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        lastUsedTime = Time.time;
        float adjustedDamage = baseDamage + player.attributes.Intelligence;
        Debug.Log(name + " on " + target.name + " heals " + adjustedDamage + ".");
        target.GetComponent<PlayerResources>().Heal(adjustedDamage);
        player.resources.UseEnergy(energyRequired);
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
        return AbilityHelper.Action.Instant;
    }
}
