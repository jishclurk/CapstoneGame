﻿using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EmptyAbility : ISpecial, IAbility {

    public string name { get; set; }
    public float effectiveRange { get; set; }
    public float baseDamage { get; set; }
    public float fireRate { get; set; }
    public bool isbasicAttack { get; set; }
    public float energyRequired { get; set; }
    public float timeToCast { get; set; }
    public float coolDownTime { get; set; }
    public float lastUsedTime { get; set; }
    public bool requiresTarget { get; set; }
    public bool requiresAim { get; set; }
    public Object aoeTarget { get; set; }
    public int id { get; private set; }
    public Image image { get; private set; }

    public int StrengthRequired { get; private set; }
    public int StaminaRequired { get; private set; }
    public int IntelligenceRequired { get; private set; }

    public EmptyAbility()
    {
        image = GameObject.Instantiate(Resources.Load("Abilities/Empty", typeof(Image))) as Image;
        id = 1;
        name = "Empty Ability";
        effectiveRange = 0.0f;
        baseDamage = 0.0f;
        fireRate = 0.0f;
        isbasicAttack = true;
        timeToCast = 0.0f;
        coolDownTime = 0.0f;
        lastUsedTime = 0.0f;
        requiresTarget = true;
        energyRequired = 0.0f;
        requiresAim = false;
        aoeTarget = null;

    }

    public void Execute(CharacterAttributes attributes, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {

    }

    public bool isReady()
    {
        return false;
    }

    public AbilityHelper.Action GetAction()
    {
        return AbilityHelper.Action.Basic;
    }
}
