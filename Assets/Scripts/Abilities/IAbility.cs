using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility {

    string name { get; set; }
    float effectiveRange { get; set; }
    float baseDamage { get; set; }
    float fireRate { get; set; }
    bool isbasicAttack { get; set; }
    float timeToCast { get; set; }
    float coolDownTime { get; set; }
    float lastUsedTime { get; set; }
    

    void Execute(GameObject origin, GameObject target);
    bool isReady();

}
