using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class SentryTurret : ISpecial, IAbility {

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
    private Object turret;

    public SentryTurret()
    {
        StrengthRequired = 0;
        StaminaRequired = 3;
        IntelligenceRequired = 3;
        image = Resources.Load("Abilities/Pistol", typeof(Image)) as Image;
        id = 18;
        name = "Sentry Turret";
        description = "Spawns a turret";
        effectiveRange = 2.0f;
        baseDamage = 0.0f;
        timeToCast = 0.0f;
        coolDownTime = 10.0f;
        effectLength = 10.0f;
        lastUsedTime = -Mathf.Infinity;
        energyRequired = 30.0f;
        aoeTarget = Resources.Load("SentryTurret/1x1YellowAuraTarget");
        turret = Resources.Load("SentryTurret/TurretObj");
        abilityObj = GameObject.FindWithTag("AbilityHelper");
    }

    public void Execute(Player player, GameObject origin, GameObject target) 
    {  
        abilityObj.GetComponent<AbilityHelper>().SentryTurretRoutine(player, target, effectLength, turret);
        player.GetComponent<NavMeshAgent>().destination = player.transform.position; //a bit funky, but I like it. Stops the user's path ONLY IF they don't have an enemy selected
        player.animController.AnimatePickup();
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

        return tm.visibleEnemies.Count > 2;
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
}
