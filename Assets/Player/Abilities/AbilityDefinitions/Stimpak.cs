using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stimpak : ISpecial, IAbility
{

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
    private Object regenField;
    private GameObject abilityObj;

    public int StrengthRequired { get; private set; }
    public int StaminaRequired { get; private set; }
    public int IntelligenceRequired { get; private set; }

    private float effectLength = 8.0f;

    public Stimpak()
    {
        StrengthRequired = 0;
        StaminaRequired = 5;
        IntelligenceRequired = 5;
        image = Resources.Load("Abilities/StimpakIcon", typeof(Image)) as Image;
        id = 18;
        name = "Stimpak";
        effectiveRange = 5.0f;
        baseDamage = 25.0f;
        timeToCast = 0.0f;
        coolDownTime = 25.0f;
        lastUsedTime = -Mathf.Infinity;
        energyRequired = 0.0f;
        aoeTarget = null;
        description = "Energy Up";
        regenField = Resources.Load("Stimpak/StimBooster");
        abilityObj = GameObject.FindWithTag("AbilityHelper");
    }

    public void Execute(Player player, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        lastUsedTime = Time.time;

        GameObject gb = Object.Instantiate(regenField, origin.transform.position, Quaternion.identity) as GameObject;
        gb.GetComponent<StayWithPlayer>().player = origin.transform;
        abilityObj.GetComponent<AbilityHelper>().StimpakRoutine(player.attributes, effectLength);

        player.resources.currentEnergy += 50;
        GameObject.Destroy(gb, 6.0f);
    }

    public void updatePassiveBonuses(CharacterAttributes attributes)
    {

    }

    public bool EvaluateCoopUse(Player player, Transform targetedEnemy, TeamManager tm)
    {
        return targetedEnemy != null && player.resources.currentEnergy < 70;
    }

    public bool isReady()
    {
        return Time.time > lastUsedTime + coolDownTime;
    }

    public void setAsReady()
    {
        lastUsedTime = -Mathf.Infinity;
    }

    public float RemainingTime()
    {
        return lastUsedTime + coolDownTime - Time.time;
    }

    public AbilityHelper.Action GetAction()
    {
        return AbilityHelper.Action.Equip;
    }

    public AbilityHelper.CoopAction GetCoopAction()
    {
        return AbilityHelper.CoopAction.Equip;
    }
}
