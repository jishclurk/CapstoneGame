using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedKit : ISpecial, IAbility
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
    private Object regenField;

    public int StrengthRequired { get; private set; }
    public int StaminaRequired { get; private set; }
    public int IntelligenceRequired { get; private set; }

    public MedKit()
    {
        StrengthRequired = 0;
        StaminaRequired = 0;
        IntelligenceRequired = 3;
        image = Resources.Load("Abilities/MedkitIcon", typeof(Image)) as Image;
        id = 12;
        name = "Heal";
        effectiveRange = 5.0f;
        baseDamage = 25.0f;
        timeToCast = 0.0f;
        coolDownTime = 8.0f;
        lastUsedTime = -Mathf.Infinity;
        energyRequired = 20.0f;
        aoeTarget = null;
        description = "Heal yourself.";
        regenField = Resources.Load("Medkit/HealBuff");
    }

    public void Execute(Player player, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        lastUsedTime = Time.time;
        float adjustedDamage = baseDamage + player.attributes.Intelligence;
        target.GetComponent<PlayerResources>().Heal(adjustedDamage);
        GameObject gb = Object.Instantiate(regenField, origin.transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject;
        gb.GetComponent<StayWithPlayer>().player = origin.transform;
        player.resources.UseEnergy(energyRequired);
        GameObject.Destroy(gb, 1.0f);
    }

    public void updatePassiveBonuses(CharacterAttributes attributes)
    {

    }

    public bool EvaluateCoopUse(Player player, Transform targetedEnemy, TeamManager tm)
    {
        return player.resources.maxHealth - player.resources.currentHealth >= baseDamage;
    }

    public bool isReady()
    {
        return Time.time > lastUsedTime + coolDownTime;
    }
    public void setAsReady()
    {
        lastUsedTime = -Mathf.Infinity;
    }

    public float RemainingTime()
    {
        return lastUsedTime + coolDownTime - Time.time;
    }

    public AbilityHelper.Action GetAction()
    {
        return AbilityHelper.Action.Equip;
    }

    public AbilityHelper.CoopAction GetCoopAction()
    {
        return AbilityHelper.CoopAction.InstantHeal;
    }
}
