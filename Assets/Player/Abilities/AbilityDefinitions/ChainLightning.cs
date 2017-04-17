using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChainLightning : ISpecial, IAbility {

    public string name { get; set; }
    public string useType { get; set; }
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
    //private GameObject abilityObj;

    public ChainLightning()
    {
        StrengthRequired = 0;
        StaminaRequired = 20;
        IntelligenceRequired = 0;
        image = Resources.Load("Abilities/ChainLightningIcon", typeof(Image)) as Image;
        id = 6;
        name = "Chain Lightning";
        description = "Lightning Strike that chains between nearby targets. Select target with left click.\n";
        effectiveRange = 9.0f;
        baseDamage = 45.0f;
        timeToCast = 0.0f;
        coolDownTime = 12.0f;
        lastUsedTime = -Mathf.Infinity;
        energyRequired = 35.0f;
        aoeTarget = null;
        bullet = Resources.Load("ChainLightning/ChainObj");
        //abilityObj = GameObject.FindWithTag("AbilityHelper");
    }

    public void Execute(Player player, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        lastUsedTime = Time.time;
        float adjustedDamage = baseDamage + (baseDamage * (player.attributes.TotalStrength - StrengthRequired) * 0.04f);

        GameObject project = Object.Instantiate(bullet, player.gunbarrel.position, Quaternion.identity) as GameObject;
        project.GetComponent<ChainProjectileScript>().destination = new Vector3(target.transform.position.x, (player.gunbarrel.position.y + target.transform.position.y) / 2, target.transform.position.z);
        project.GetComponent<ChainProjectileScript>().targetedEnemy = target;
        project.GetComponent<ChainProjectileScript>().damage = adjustedDamage;
        project.GetComponent<ChainProjectileScript>().enemyPool = player.watchedEnemies;
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
        return player.watchedEnemies.Count > 2;
    }

    public float RemainingTime()
    {
        return lastUsedTime + coolDownTime - Time.time;
    }

    public AbilityHelper.Action GetAction()
    {
        return AbilityHelper.Action.Target;
    }

    public AbilityHelper.CoopAction GetCoopAction()
    {
        return AbilityHelper.CoopAction.TargetHurt;
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
        string requires = "Requires: ";
        if (strReq.Length == 0 && intReq.Length == 0 && stmReq.Length == 0)
        {
            requires = " ";
        }

        return description + requires + strReq + intReq + stmReq + "\nFirst Impact: " + Mathf.Floor((baseDamage + (baseDamage * (p.attributes.TotalStrength - StrengthRequired) * 0.04f))) + " damage.\nCooldown: " + coolDownTime + " seconds.";
    }
}
