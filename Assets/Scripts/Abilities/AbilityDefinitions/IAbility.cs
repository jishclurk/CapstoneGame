using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IAbility {

    string name { get; set; }
    string description { get; set; }
    int id { get; }
    Image image { get; }
    float effectiveRange { get; }

    void Execute(CharacterAttributes attributes, GameObject origin, GameObject target);
    bool isReady();
    AbilityHelper.Action GetAction();

}
