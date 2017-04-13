using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeechDart : ISpecial, IAbility {

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
    private Object regenField;

    public LeechDart()
    {
        StrengthRequired = 5;
        StaminaRequired = 0;
        IntelligenceRequired = 5;
        image = Resources.Load("Abilities/LeechDartIcon", typeof(Image)) as Image;
        id = 11;
        name = "LeechDart";
        effectiveRange = 9.0f;
        baseDamage = 40.0f;
        timeToCast = 0.0f;
        coolDownTime = 15.0f;
        lastUsedTime = -Mathf.Infinity;
        energyRequired = 30.0f;
        aoeTarget = null;
        description = "Shoots a dart that saps enemy health. Select target with left click. ";
        bullet = Resources.Load("LeechDart/DartProjectile");
        regenField = Resources.Load("LeechDart/DartBooster");
    }

    public void Execute(Player player, GameObject origin, GameObject target) 
    {
        lastUsedTime = Time.time;
        float adjustedDamage = baseDamage + (baseDamage * (player.attributes.TotalStrength - StrengthRequired) * 0.04f);
        target.GetComponent<EnemyHealth>().TakeDamage(adjustedDamage);
        player.GetComponent<PlayerResources>().Heal(adjustedDamage / 2);

        GameObject project = Object.Instantiate(bullet, player.gunbarrel.position, Quaternion.identity) as GameObject;
        project.GetComponent<DartProjectileScript>().destination = new Vector3(target.transform.position.x, (player.gunbarrel.position.y + target.transform.position.y) / 2, target.transform.position.z);
        project.GetComponent<DartProjectileScript>().targetedEnemy = target;
        player.resources.UseEnergy(energyRequired);


        GameObject gb = Object.Instantiate(regenField, origin.transform.position, Quaternion.identity) as GameObject;
        gb.GetComponent<StayWithPlayer>().player = origin.transform;
        GameObject.Destroy(gb, 2.0f);
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
        if(targetedEnemy != null)
        {
            EnemyHealth eh = targetedEnemy.GetComponent<EnemyHealth>();
            use = eh.maxHealth - eh.currentHealth > baseDamage/2;
        }
        return use;
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

        return description + "Requires: " + strReq + intReq + stmReq + " Damage " + Mathf.Floor(baseDamage + (baseDamage * (p.attributes.TotalStrength - StrengthRequired) * 0.04f)) + ". Cooldown: " + coolDownTime + " seconds.";
    }
}
