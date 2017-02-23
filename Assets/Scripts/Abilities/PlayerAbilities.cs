using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour {

    //binding from key to spot in the ability array
    public Dictionary<KeyCode, int> AbilityBindings;

    //array that corresponds with ability bar in HUB
    public IAbility[] abilityArray;

    //possibly eleminate these and just keep track of abilites in the array??
    public IAbility one { get; set; }
    public IAbility two { get; set; }
    public IAbility three { get; set; }
    public IAbility four { get; set; }
    public IAbility five { get; set; }
    public IAbility Basic { get; set; }
    public List<IAbility> unlockedAbilities { get; set; }

    public void Awake()
    {
        AbilityBindings = new Dictionary<KeyCode, int>();

        one = new EmptyAbility();
        two = new EmptyAbility();
        three = new EmptyAbility();
        four = new EmptyAbility();
        five = new EmptyAbility();
        Basic = new EmptyAbility();

        unlockedAbilities = new List<IAbility>();
        LoadUnlockedAbilities();
        LoadHotBar();

        abilityArray = new IAbility[5] { one, two, three, four, five };
        SetDefaultBindings();
    }

    private void LoadUnlockedAbilities()
    {
        //some sort of reading from save file would happen here
        unlockedAbilities.Add(new PistolShot());
        unlockedAbilities.Add(new Zap());
        unlockedAbilities.Add(new SelfHeal());
        unlockedAbilities.Add(new AOE());
        unlockedAbilities.Add(new GrenadeThrow());
    }

    private void LoadHotBar()
    {
        //some sort of reading from save file would happen here
        Basic = unlockedAbilities[0];
        one = unlockedAbilities[1];
        two = unlockedAbilities[2];
        three = unlockedAbilities[3];
        four = unlockedAbilities[4];
    }

    public void SetDefaultBindings()
    {
        AbilityBindings[KeyCode.Q] = 0;
        AbilityBindings[KeyCode.W] = 1;
        AbilityBindings[KeyCode.E] = 2;
        AbilityBindings[KeyCode.R] = 3;
        AbilityBindings[KeyCode.T] = 4;
    }

    public void SetNewBinding(KeyCode key, int spot)
    {
        AbilityBindings[key] = spot;
    }

    public void SetNewAbility(IAbility ability, int spot)
    {
        abilityArray[spot] = ability;
    }




}
