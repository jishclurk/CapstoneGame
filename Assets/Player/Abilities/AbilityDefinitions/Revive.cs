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
    private Object bullet;

    public Revive()
    {
        StrengthRequired = 0;
        StaminaRequired = 0;
        IntelligenceRequired = 6;
        image = Resources.Load("Abilities/Zap", typeof(Image)) as Image;
        id = 17;
        name = "Revive";
        effectiveRange = 1.8f;
        baseDamage = 50.0f;
        timeToCast = 0.0f;
        coolDownTime = 10.0f;
        lastUsedTime = -Mathf.Infinity;
        energyRequired = 40.0f;
        aoeTarget = null;
        description = "A quick Zap from your gun.";
    }

    public void Execute(Player player, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        lastUsedTime = Time.time;
        float adjustedDamage = baseDamage + player.attributes.Intelligence * 0.1f;
        

        Vector3 playerToTarget = target.transform.position - player.gunbarrel.position;
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
}
