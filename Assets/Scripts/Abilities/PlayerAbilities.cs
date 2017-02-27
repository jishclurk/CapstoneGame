using System;
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

    //possibly eleminate these and just keep track of abilites in the array??
    public ISpecial one { get; set; }
    public ISpecial two { get; set; }
    public ISpecial three { get; set; }
    public ISpecial four { get; set; }
    public IBasic Basic { get; set; }
    public List<ISpecial> unlockedAbilities { get; set; }

    public void Awake()
    {
        AbilityBindings = new Dictionary<KeyCode, int>();

        one = new EmptyAbility();
        two = new EmptyAbility();
        three = new EmptyAbility();
        four = new EmptyAbility();
        Basic = new PistolShot();

        unlockedAbilities = new List<ISpecial>();
        LoadUnlockedAbilities();
        LoadHotBar();

        abilityArray = new ISpecial[4] { one, two, three, four };
        Debug.Log(abilityArray);
        SetDefaultBindings();
    }

    private void LoadUnlockedAbilities()
    {
        //some sort of reading from save file would happen here
        //unlockedAbilities.Add(new PistolShot());
        unlockedAbilities.Add(new Zap());
        unlockedAbilities.Add(new SelfHeal());
       // unlockedAbilities.Add(new AOE());
        unlockedAbilities.Add(new GrenadeThrow());
    }

    private void LoadHotBar()
    {
        //some sort of reading from save file would happen here
      //  Basic = unlockedAbilities[0];
        one = unlockedAbilities[0];
        two = unlockedAbilities[1];
        three = unlockedAbilities[2];
        //four = unlockedAbilities[4];
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


}

