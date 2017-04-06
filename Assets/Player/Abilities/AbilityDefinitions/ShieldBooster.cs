using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBooster : ISpecial, IAbility {

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

    private float effectLength = 10.0f;
    private Object booster;

    public ShieldBooster()
    {
        StrengthRequired = 0;
        StaminaRequired = 10;
        IntelligenceRequired = 0;
        image = Resources.Load("Abilities/ShieldBoosterIcon", typeof(Image)) as Image;
        id = 16;
        name = "Shield Booster";
        description = "Sets all players' defense to max within the effect circle";
        effectiveRange = 15.0f;
        baseDamage = 0.0f;
        timeToCast = 0.0f;
        coolDownTime = 5.0f;
        lastUsedTime = -Mathf.Infinity;
        energyRequired = 30.0f;
        aoeTarget = Resources.Load("ShieldBooster/4x4BlueAuraTarget");
        booster = Resources.Load("ShieldBooster/Booster");
        abilityObj = GameObject.FindWithTag("AbilityHelper");
    }

    public void Execute(Player player, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {  
        abilityObj.GetComponent<AbilityHelper>().ShieldBoosterRoutine(player.attributes, target, effectLength, booster);
        player.animController.AnimateUse(0.35f);

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

        return player.attributes.Stamina < 20 && tm.visibleEnemies.Count > 1 || player.resources.currentHealth < 50 || tm.visibleEnemies.Count > 4;
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
        return AbilityHelper.CoopAction.AOEHeal;
    }
}
