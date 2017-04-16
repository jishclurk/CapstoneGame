using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Revive : ISpecial, IAbility {

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
    private Object regenField;


    public Revive()
    {
        StrengthRequired = 0;
        StaminaRequired = 0;
        IntelligenceRequired = 10;
        image = Resources.Load("Abilities/ReviveIcon", typeof(Image)) as Image;
        id = 13;
        name = "Revive";
        effectiveRange = 1.8f;
        baseDamage = 50.0f;
        timeToCast = 0.0f;
        coolDownTime = 20.0f;
        lastUsedTime = -Mathf.Infinity;
        energyRequired = 40.0f;
        aoeTarget = null;
        description = "Revive a downed teammate and heal them. Can also be used on alive teammates. ";
        regenField = Resources.Load("Revive/RevBuff");
    }

    public void Execute(Player player, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        lastUsedTime = Time.time;
        float adjustedDamage = baseDamage + (baseDamage * (player.attributes.TotalIntelligence - IntelligenceRequired) * 0.03f);


        PlayerResources friendHealth = target.GetComponent<PlayerResources>();
        if (friendHealth.isDead)
        {
            player.animController.AnimatePickup();
            friendHealth.Revive();
            friendHealth.Heal(adjustedDamage);
        } else
        {
            player.animController.AnimateUse(0.5f);
            friendHealth.Heal(adjustedDamage);
        }

        GameObject gb = Object.Instantiate(regenField, target.transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject;
        gb.GetComponent<StayWithPlayer>().player = target.transform;


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
        bool use = false;
        foreach(Player p in tm.playerList)
        {
            if (p.resources.isDead)
            {
                use = true;
                break;
            }
        }
        return use;
    }

    public float RemainingTime()
    {
        return lastUsedTime + coolDownTime - Time.time;
    }

    public AbilityHelper.Action GetAction()
    {
        return AbilityHelper.Action.TargetFriend;
    }

    public AbilityHelper.CoopAction GetCoopAction()
    {
        return AbilityHelper.CoopAction.TargetHeal;
    }

    public string GetHoverDescription(Player p)
    {
        string strReq = "";
        string intReq = "";
        string stmReq = "";
        if (p.attributes.Strength < StrengthRequired)
        {
            strReq = StrengthRequired + " " + "STR. ";
        }
        if (p.attributes.Intelligence < IntelligenceRequired)
        {
            intReq = IntelligenceRequired + " " + "INT. ";
        }
        if (p.attributes.Stamina < StaminaRequired)
        {
            stmReq = StaminaRequired + " " + "STM. ";
        }
        string requires = "Requires: ";
        if (strReq.Length == 0 && intReq.Length == 0 && stmReq.Length == 0)
        {
            requires = " ";
        }

        return description + requires + strReq + intReq + stmReq + " Heal Amount " + Mathf.Max(Mathf.Floor(baseDamage + (baseDamage * (p.attributes.TotalIntelligence - IntelligenceRequired))), 0.0f) + " Cooldown: " + coolDownTime + " seconds.";
    }
}
