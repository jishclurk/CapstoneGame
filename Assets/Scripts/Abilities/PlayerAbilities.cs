using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : ICharacterAbilities {

    public IAbility Q { get; set; }
    public IAbility W { get; set; }
    public IAbility E { get; set; }
    public IAbility R { get; set; }
    public IAbility T { get; set; }
    public IAbility Basic { get; set; }
    public List<IAbility> unlockedAbilities;

    public PlayerAbilities()
    {
        Q = new EmptyAbility();
        W = new EmptyAbility();
        E = new EmptyAbility();
        R = new EmptyAbility();
        T = new EmptyAbility();
        Basic = new EmptyAbility();
        unlockedAbilities = new List<IAbility>();
        LoadUnlockedAbilities();
        LoadHotBar();
    }

    private void LoadUnlockedAbilities()
    {
        //some sort of reading from save file would happen here
        unlockedAbilities.Add(new PistolShot());
        unlockedAbilities.Add(new Zap());
        unlockedAbilities.Add(new SelfHeal());
    }

    private void LoadHotBar()
    {
        //some sort of reading from save file would happen here
        Basic = unlockedAbilities[0];
        Q = unlockedAbilities[1];
        W = unlockedAbilities[2];
    }

}
