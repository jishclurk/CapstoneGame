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

    //Basic gun ability
    public IBasic Basic { get; set; }

    //List of unlocked basic abilities
    public List<IBasic> unlockedBasics = new List<IBasic>();

    //List of all available abilities
    public List<ISpecial> unlockedSpecialAbilities { get; set; }

    //List of all potenital basic abilities
    private List<IBasic> potentialBasics; 

    //List of all potentail specail abilities
    private List<ISpecial> potentialSpecials; 

    public void Awake()
    {
        potentialSpecials = new List<ISpecial>() { new EmptyAbility(), new GrenadeThrow(), new SelfHeal(), new Zap() };
        potentialBasics = new List<IBasic>() { new PistolShot() };
        AbilityBindings = new Dictionary<KeyCode, int>();

        Basic = new PistolShot();

        unlockedSpecialAbilities = new List<ISpecial>();
        LoadUnlockedAbilities();
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
        unlockedSpecialAbilities.Add(new Zap());
        unlockedSpecialAbilities.Add(new SelfHeal());
        unlockedSpecialAbilities.Add(new GrenadeThrow());
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
            abilityArray[1] = new SelfHeal();
            abilityArray[2] = new GrenadeThrow();
            abilityArray[3] = new EmptyAbility();
        }
    }

    public void UpdateUnlockedAbilities(CharacterAttributes attributes)
    {
        //for(int i = potentialBasics.Count -1; i<=0; i--)
        //{
        //    IBasic potential = potentialBasics[i];
        //    if(attributes.Strength>= potential.StrengthRequired && attributes.Stamina>= potential.StaminaRequired && attributes.Intelligence>= potential.StaminaRequired)
        //    {
        //        potentialBasics.Remove(potential);
        //        unlockedBasics.Add(potential);
        //    }
            
        //}

        //for (int i = potentialSpecials.Count - 1; i <= 0; i--)
        //{
        //    ISpecial potential = potentialSpecials[i];
        //    if (attributes.Strength >= potential.StrengthRequired && attributes.Stamina >= potential.StaminaRequired && attributes.Intelligence >= potential.StaminaRequired)
        //    {
        //        potentialSpecials.Remove(potential);
        //        unlockedSpecialAbilities.Add(potential);
        //    }

        //}
    }


}

