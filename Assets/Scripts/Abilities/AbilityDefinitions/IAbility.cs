using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IAbility {

    string name { get; set; }
    Image image { get; }
    int id { get; }
    float effectiveRange { get; }

    int StrengthRequired { get; }
    int StaminaRequired { get; }
    int IntelligenceRequired { get; }


    void Execute(CharacterAttributes attributes, GameObject origin, GameObject target);
    bool isReady();
    AbilityHelper.Action GetAction();

}
