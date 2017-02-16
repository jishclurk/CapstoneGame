using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterAbilities {

    IAbility Q { get; set; }
    IAbility W { get; set; }
    IAbility E { get; set; }
    IAbility R { get; set; }
    IAbility T { get; set; }
    IAbility Basic { get; set; }
    List<IAbility> unlockedAbilities { get; set; }
}
