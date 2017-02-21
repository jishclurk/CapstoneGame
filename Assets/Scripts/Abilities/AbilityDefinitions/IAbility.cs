using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IAbility {

    string name { get; set; }
    float effectiveRange { get; set; }
    float baseDamage { get; set; }
    float fireRate { get; set; }
    bool isbasicAttack { get; set; }
    bool requiresTarget { get; set; }
    bool requiresAim { get; set; }
    float energyRequired { get; set; }
    float timeToCast { get; set; }
    float coolDownTime { get; set; }
    float lastUsedTime { get; set; }
    Object aoeTarget { get; set; }

    Image image { get; }
    int id { get; }

    void Execute(CharacterAttributes attributes, GameObject origin, GameObject target);
    bool isReady();

}
