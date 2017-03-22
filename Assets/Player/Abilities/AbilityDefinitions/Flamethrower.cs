using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flamethrower : ISpecial, IAbility {

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



    private AbilityHelper ah;
    private Object flame;
    private float lastUsedTime;



    public int StrengthRequired { get; private set; }
    public int StaminaRequired { get; private set; }
    public int IntelligenceRequired { get; private set; }



    public Flamethrower()
    {
        StrengthRequired = 5;
        StaminaRequired = 0;
        IntelligenceRequired = 0;
        image = Resources.Load("Abilities/Pistol", typeof(Image)) as Image;
        id = 10;
        name = "Flamethrower";
        effectiveRange = 6.0f;
        baseDamage = 8.0f;
        timeToCast = 5.0f;
        coolDownTime = 20.0f;
        lastUsedTime = -Mathf.Infinity;
        energyRequired = 50.0f;
        ah = GameObject.FindWithTag("AbilityHelper").GetComponent<AbilityHelper>();
        flame = Resources.Load("FlameThrower/yFlame");

    }

    public void Execute(Player player, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        lastUsedTime = Time.time;
        baseDamage = baseDamage + (player.attributes.Strength * 0.5f);
        ah.FlameThrowerRoutine(player, origin, target, flame, baseDamage, effectiveRange);

        player.resources.UseEnergy(energyRequired);
    }

    public bool EvaluateCoopUse(Player player, Transform targetedEnemy, TeamManager tm)
    {

        return targetedEnemy != null && Vector3.Distance(targetedEnemy.position, player.transform.position) < effectiveRange;
    }

    public void updatePassiveBonuses(CharacterAttributes attributes)
    {

    }

    public float RemainingTime()
    {
        return lastUsedTime + coolDownTime - Time.time;
    }

    public bool isReady()
    {
        return Time.time > lastUsedTime + coolDownTime;
    }

    public AbilityHelper.Action GetAction()
    {
        return AbilityHelper.Action.Equip;
    }

    public AbilityHelper.CoopAction GetCoopAction()
    {
        return AbilityHelper.CoopAction.Equip;
    }
}
