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
        StrengthRequired = 5;
        StaminaRequired = 0;
        IntelligenceRequired = 0;
        image = Resources.Load("Abilities/GrenadeThrowIcon", typeof(Image)) as Image;
        id = 9;
        name = "Grenade Throw";
        description = "A fragmentation grenade that deals high damage within the blast radius. ";
        effectiveRange = 10.0f;
        baseDamage = 30.0f;
        timeToCast = 1.0f;
        coolDownTime = 10.0f;
        lastUsedTime = -Mathf.Infinity;
        energyRequired = 25.0f;
        aoeTarget = Resources.Load("GrenadeThrow/3x3RedAuraTarget");
        explosion = Resources.Load("GrenadeThrow/explosion");
        abilityObj = GameObject.FindWithTag("AbilityHelper");
        grenade = Resources.Load("GrenadeThrow/nade");
    }

    public void Execute(Player player, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        float adjustedDamage = baseDamage + (baseDamage * (player.attributes.TotalStrength - StrengthRequired) * 0.04f);
        abilityObj.GetComponent<AbilityHelper>().GrenadeThrowRoutine(player.attributes, origin, target, adjustedDamage, explosion, timeToCast, grenade);
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
        
        return targetedEnemy != null;
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

        return description + requires + strReq + intReq + stmReq + "Damage: " + Mathf.Floor((baseDamage + (baseDamage * (p.attributes.TotalStrength - StrengthRequired) * 0.04f))) + ". Cooldown: " + coolDownTime + " seconds."; 
    }
}
