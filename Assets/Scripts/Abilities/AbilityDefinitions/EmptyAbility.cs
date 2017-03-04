using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EmptyAbility : ISpecial, IAbility {

    public string name { get; set; }
    public string description { get; set; }
    public int id { get; private set; }
    public Image image { get; private set; }
    public float effectiveRange { get; set; }
    public float baseDamage { get; set; }
    public float energyRequired { get; set; }
    public float timeToCast { get; set; }
    public float coolDownTime { get; set; }
    public Object aoeTarget { get; set; }

    public int StrengthRequired { get; private set; }
    public int StaminaRequired { get; private set; }
    public int IntelligenceRequired { get; private set; }

    public EmptyAbility()
    {
        StrengthRequired = 0;
        StaminaRequired = 0;
        IntelligenceRequired = 0;
        image = GameObject.Instantiate(Resources.Load("Abilities/Empty", typeof(Image))) as Image;
        id = 1;
        name = "Empty Ability";
        effectiveRange = 0.0f;
        baseDamage = 0.0f;
        timeToCast = 0.0f;
        coolDownTime = 0.0f;
        energyRequired = 0.0f;
        aoeTarget = null;

    }

    public void Execute(Player player, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {

    }

    public bool isReady()
    {
        return false;
    }

    public float RemainingTime()
    {
        return 0;
    }

    public AbilityHelper.Action GetAction()
    {
        return AbilityHelper.Action.Basic;
    }

    public AbilityHelper.CoopAction GetCoopAction()
    {
        return AbilityHelper.CoopAction.Basic;
    }

}
