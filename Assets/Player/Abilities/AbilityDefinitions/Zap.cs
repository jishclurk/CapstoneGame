using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zap : ISpecial, IAbility {

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

    public Zap()
    {
        StrengthRequired = 0;
        StaminaRequired = 3;
        IntelligenceRequired = 0;
        image = Resources.Load("Abilities/ZapIcon", typeof(Image)) as Image;
        id = 4;
        name = "Zap";
        effectiveRange = 9.0f;
        baseDamage = 40.0f;
        timeToCast = 0.0f;
        coolDownTime = 10.0f;
        lastUsedTime = -Mathf.Infinity;
        energyRequired = 30.0f;
        aoeTarget = null;
        description = "A quick Zap from your gun.";
        bullet = Resources.Load("Zap/ZapObj");
    }

    public void Execute(Player player, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        lastUsedTime = Time.time;
        float adjustedDamage = baseDamage + player.attributes.Strength * 0.1f;
        target.GetComponent<EnemyHealth>().TakeDamage(adjustedDamage);
        target.GetComponent<EnemyHealth>().TakeStunDamage(1);

        Vector3 playerToTarget = target.transform.position - player.gunbarrel.position;
        GameObject project = Object.Instantiate(bullet, player.gunbarrel.position, Quaternion.identity) as GameObject;
        project.GetComponent<ZapProjectileScript>().destination = new Vector3(target.transform.position.x, (player.gunbarrel.position.y + target.transform.position.y) / 2, target.transform.position.z);
        project.GetComponent<ZapProjectileScript>().targetedEnemy = target;
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
}
