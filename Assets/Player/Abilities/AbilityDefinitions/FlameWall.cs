using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class FlameWall : ISpecial, IAbility {

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

    private float effectLength;
    private Object wall;

    public FlameWall()
    {
        StrengthRequired = 5;
        StaminaRequired = 5;
        IntelligenceRequired = 0;
        image = Resources.Load("Abilities/FlameWallIcon", typeof(Image)) as Image;
        id = 8;
        effectLength = 8.0f;
        name = "Flame Wall";
        description = "Shoots a wall of fire. Deals burn damage. Lasts " + Mathf.Floor(effectLength) + " seconds. ";
        effectiveRange = 5.5f;
        baseDamage = 30.0f;
        timeToCast = 0.0f;
        coolDownTime = 15.0f;
        lastUsedTime = -Mathf.Infinity;
        energyRequired = 70.0f;
        aoeTarget = Resources.Load("FlameWall/4x1RedAuraTarget");
        wall = Resources.Load("FlameWall/FlameWallObj");
        abilityObj = GameObject.FindWithTag("AbilityHelper");
    }

    public void Execute(Player player, GameObject origin, GameObject target) 
    {
        float adjustedDamage = baseDamage + (baseDamage * (player.attributes.TotalStrength - StrengthRequired) * 0.04f);
        abilityObj.GetComponent<AbilityHelper>().FlameWallRoutine(player, target, adjustedDamage, effectLength, wall);
        player.animController.AnimateUse(0.4f);
        lastUsedTime = Time.time;
        player.resources.UseEnergy(energyRequired);

    }

    public void updatePassiveBonuses(CharacterAttributes attributes)
    {

    }

    public void setAsReady()
    {
        lastUsedTime = -Mathf.Infinity;
    }

    public bool isReady()
    {
        return Time.time > lastUsedTime + coolDownTime;
    }

    public bool EvaluateCoopUse(Player player, Transform targetedEnemy, TeamManager tm)
    {

        return tm.visibleEnemies.Count > 1;
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
