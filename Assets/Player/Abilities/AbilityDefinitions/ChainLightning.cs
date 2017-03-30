using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChainLightning : ISpecial, IAbility {

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
    private GameObject abilityObj;

    public ChainLightning()
    {
        StrengthRequired = 0;
        StaminaRequired = 6;
        IntelligenceRequired = 0;
        image = Resources.Load("Abilities/Zap", typeof(Image)) as Image;
        id = 23;
        name = "Chain Lightning";
        effectiveRange = 9.0f;
        baseDamage = 35.0f;
        timeToCast = 0.0f;
        coolDownTime = 10.0f;
        lastUsedTime = -Mathf.Infinity;
        energyRequired = 30.0f;
        aoeTarget = null;
        description = "A quick Zap from your gun.";
        bullet = Resources.Load("ChainLightning/ChainObj");
        abilityObj = GameObject.FindWithTag("AbilityHelper");
    }

    public void Execute(Player player, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        lastUsedTime = Time.time;
        float adjustedDamage = baseDamage + player.attributes.Strength * 0.1f;

        Vector3 playerToTarget = target.transform.position - player.gunbarrel.position;
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
}
