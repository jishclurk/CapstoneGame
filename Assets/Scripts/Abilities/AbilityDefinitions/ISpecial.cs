using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpecial : IAbility {

    float baseDamage { get;  }
    float energyRequired { get;  }
    float coolDownTime { get; }
    float timeToCast { get; }
    Object aoeTarget { get; set; }

}
