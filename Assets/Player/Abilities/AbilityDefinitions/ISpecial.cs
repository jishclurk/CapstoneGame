using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpecial : IAbility {

    float baseDamage { get;  }
    float energyRequired { get;  }
    float coolDownTime { get; }
    float timeToCast { get; }
    Object aoeTarget { get; set; }

    float RemainingTime();
    void setAsReady();
    void updatePassiveBonuses(CharacterAttributes attributes);
    bool EvaluateCoopUse(Player player, Transform targetedEnemy, TeamManager tm);

}
