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
        StrengthRequired = 2;
        StaminaRequired = 2;
        IntelligenceRequired = 0;
        image = Resources.Load("Abilities/FlameWallIcon", typeof(Image)) as Image;
        id = 8;
        name = "Flame Wall";
        description = "Spawns a fiery wall";
        effectiveRange = 5.5f;
        baseDamage = 55.0f;
        timeToCast = 0.0f;
        coolDownTime = 10.0f;
        effectLength = 10.0f;
        lastUsedTime = -Mathf.Infinity;
        energyRequired = 30.0f;
        aoeTarget = Resources.Load("FlameWall/4x1RedAuraTarget");
        wall = Resources.Load("FlameWall/FlameWallObj");
        abilityObj = GameObject.FindWithTag("AbilityHelper");
    }

    public void Execute(Player player, GameObject origin, GameObject target) 
    {  
        abilityObj.GetComponent<AbilityHelper>().FlameWallRoutine(player, target, baseDamage, effectLength, wall);
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
}
