using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BioGrenade : ISpecial, IAbility {

    public string name { get; set; }
    public string description { get; set; }
    public float effectiveRange { get; set; }
    public float baseDamage { get; set; }
    public float energyRequired { get; set; }
    public float timeToCast { get; set; }
    public float coolDownTime { get; set; }

    public Object aoeTarget { get; set; }
    public int id { get; private set; }
    public Image image { get; private set; }

    private float lastUsedTime;
    private Object bubble;
    private GameObject abilityObj;
    private float healRate;
    private float healLength;

    public int StrengthRequired { get; private set; }
    public int StaminaRequired { get; private set; }
    public int IntelligenceRequired { get; private set; }

    public BioGrenade()
    {
        StrengthRequired = 0;
        StaminaRequired = 0;
        IntelligenceRequired = 6;
        image = Resources.Load("Abilities/BioGrenade", typeof(Image)) as Image;
        id = 0;
        name = "Bio Grenade";
        effectiveRange = 8.0f;
        baseDamage = 2.0f;
        timeToCast = 0.0f;
        coolDownTime = 1.0f;
        lastUsedTime = -Mathf.Infinity;
        energyRequired = 50.0f;
        aoeTarget = Resources.Load("BioGrenade/5x5GreenAuraTarget");
        bubble = Resources.Load("BioGrenade/HealBubble");
        abilityObj = GameObject.FindWithTag("AbilityHelper");

        healRate = 0.5f;
        healLength = 12.0f;
    }

    public void Execute(Player player, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        abilityObj.GetComponent<AbilityHelper>().BioGrenadeRoutine(player.attributes, origin, target, baseDamage, healRate, healLength, timeToCast, bubble);

        lastUsedTime = Time.time;

        origin.GetComponent<PlayerResources>().UseEnergy(energyRequired);

    }


    public bool isReady()
    {
        return Time.time > lastUsedTime + coolDownTime;
    }

    public void setAsReady()
    {
        lastUsedTime = -Mathf.Infinity;
    }

    public bool EvaluateCoopUse(Player player, Transform targetedEnemy, TeamManager tm)
    {
        int count = 0;
        foreach(Player p in tm.playerList)
        {
            if(p.resources.maxHealth - p.resources.currentHealth >= (baseDamage*(healLength/healRate)*0.8)) //if at least 2 players could use 80% of max healing ability
            {
                count++;
            }
        }
        return count >= 2;
    }

    public void updatePassiveBonuses(CharacterAttributes attributes)
    {

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
        return AbilityHelper.CoopAction.AOEHeal;
    }
    

}
