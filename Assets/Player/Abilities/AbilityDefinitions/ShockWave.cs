﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shockwave : ISpecial, IAbility {

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
    private GameObject abilityObj;

    public int StrengthRequired { get; private set; }
    public int StaminaRequired { get; private set; }
    public int IntelligenceRequired { get; private set; }

    private Object blast;

    public Shockwave()
    {
        StrengthRequired = 10;
        StaminaRequired = 10;
        IntelligenceRequired = 0;
        image = Resources.Load("Abilities/ShockwaveIcon", typeof(Image)) as Image;
        id = 17;
        name = "Shockwave";
        description = "Knock back enemies within the AOE. Stuns enemies and deals minor damage. ";
        effectiveRange = 0.05f;
        baseDamage = 10.0f;
        timeToCast = 0.0f;
        coolDownTime = 15.0f;
        lastUsedTime = -Mathf.Infinity;
        energyRequired = 35.0f;
        aoeTarget = Resources.Load("Shockwave/4x4BlueAuraTarget");
        blast = Resources.Load("Shockwave/blast");
        abilityObj = GameObject.FindWithTag("AbilityHelper");
    }

    public void Execute(Player player, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        float adjustedDamage = baseDamage + (baseDamage * (player.attributes.TotalStrength - StrengthRequired) * 0.04f);
        abilityObj.GetComponent<AbilityHelper>().ShockwaveRoutine(player.transform, target, blast, adjustedDamage, 3.0f);
        player.animController.AnimatePickup();

        lastUsedTime = Time.time;
        player.resources.UseEnergy(energyRequired);

    }

    public void updatePassiveBonuses(CharacterAttributes attributes)
    {

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
        if(player.visibleEnemies.Count > 2)
        {
            foreach(GameObject enemy in player.visibleEnemies)
            {
                if (Vector3.Distance(enemy.transform.position, player.transform.position) < 3.0f)
                    count++;
            }
        }

        return count > 2;
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

    public string GetHoverDescription(Player p)
    {
        string strReq = "";
        string intReq = "";
        string stmReq = "";
        if (StrengthRequired > 0)
        {
            strReq = StrengthRequired + " " + "STR. ";
        }
        if (IntelligenceRequired > 0)
        {
            intReq = IntelligenceRequired + " " + "INT. ";
        }
        if (StaminaRequired > 0)
        {
            stmReq = StaminaRequired + " " + "STM. ";
        }

        return description + "Requires: " + strReq + intReq + stmReq + "Damage: " + Mathf.Floor((baseDamage + (baseDamage * (p.attributes.TotalStrength - StrengthRequired) * 0.04f))) + ". Cooldown: " + coolDownTime + " seconds.";
    }
}
