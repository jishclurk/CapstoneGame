﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAbilities : MonoBehaviour
{

    //binding from key to spot in the ability array
    public Dictionary<KeyCode, int> AbilityBindings;

    //array that corresponds with ability bar in HUB
    public ISpecial[] abilityArray;

    //Basic gun ability
    public IBasic Basic { get; set; }

    //List of all available abilities
    public List<IAbility> unlockedAbilities { get; set; }

    //List of all potentail specail abilities
    private List<IAbility> potentialAbilities;

    public void Awake()
    {
        potentialAbilities = new List<IAbility>() {new GrenadeThrow(), new MedKit(), new Zap(), new BioGrenade(), new RIPRounds(),
            new ShieldBooster(), new Revive(), new SentryTurret(), new Flamethrower(), new Stimpak(), new Invigorate(),
            new ChainLightning(), new Shockwave(), new LeechDart(), new FlameWall(), new SniperShot(), new SMGShot()};
        AbilityBindings = new Dictionary<KeyCode, int>();

        Basic = new RifleShot();

        unlockedAbilities = new List<IAbility>();
        //LoadUnlockedAbilities();
        // LoadHotBar();

        abilityArray = new ISpecial[4];// { one, two, three, four };
        for (int i = 0; i < abilityArray.Length; i++)
        {
            abilityArray[i] = new EmptyAbility();
        }
        Debug.Log(abilityArray);
        SetDefaultBindings();
    }

    private void LoadUnlockedAbilities()
    {
        unlockedAbilities.Add(new Zap());
        unlockedAbilities.Add(new MedKit());
        unlockedAbilities.Add(new GrenadeThrow());
    }

    public void setKeysFromSettings()
    {

    }

    public void SetDefaultBindings()
    {
        AbilityBindings[KeyCode.Q] = 0;
        AbilityBindings[KeyCode.W] = 1;
        AbilityBindings[KeyCode.E] = 2;
        AbilityBindings[KeyCode.R] = 3;
    }

    public void SetNewBinding(KeyCode key, int spot)
    {
        AbilityBindings[key] = spot;
    }

    public void SetNewAbility(ISpecial ability, int spot)
    {
        abilityArray[spot] = ability;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            abilityArray[0] = new Zap();
            abilityArray[1] = new Flamethrower();
            abilityArray[2] = new GrenadeThrow();
            abilityArray[3] = new BioGrenade();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Basic = new ShotgunShot();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Basic = new SMGShot();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Basic = new SniperShot();
        }
    }

    public void UpdateUnlockedAbilities(CharacterAttributes attributes)
    {
        //foreach(IAbility x in unlockedAbilities)
        //{
        //    Debug.Log(x);
        //}
        /* 
         * for (int i = potentialBasics.Count - 1; i >= 0; i--)
        {
            IBasic potential = potentialBasics[i];
            if (attributes.Strength >= potential.StrengthRequired && attributes.Stamina >= potential.StaminaRequired && attributes.Intelligence >= potential.StaminaRequired)
            {
                potentialBasics.Remove(potential);
                unlockedBasics.Add(potential);
            }

        }
        */

        for (int i = potentialAbilities.Count - 1; i>= 0; i--)
        {
            IAbility potential = potentialAbilities[i];
            if (attributes.Strength >= potential.StrengthRequired && attributes.Stamina >= potential.StaminaRequired && attributes.Intelligence >= potential.IntelligenceRequired)
            {
                potentialAbilities.Remove(potential);
                unlockedAbilities.Add(potential);
            }

        }

        //foreach (IAbility x in unlockedAbilities)
        //{
        //    Debug.Log(x);
        //}
    }


}

