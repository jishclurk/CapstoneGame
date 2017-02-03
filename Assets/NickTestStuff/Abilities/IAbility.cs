using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility {

    float effectiveRange { get; set; }
    float baseDamage { get; set; }
    float fireRate { get; set; }
    bool repeating { get; set; }

    void Execute(GameObject origin, GameObject target);

}
