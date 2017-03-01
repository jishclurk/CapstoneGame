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

    //float effectiveRange { get; }
    //float baseDamage { get;  }
    //float fireRate { get;  }
    //bool isbasicAttack { get;  }
    //bool requiresTarget { get;  }
    //bool requiresAim { get;  }
    //float energyRequired { get; }
    //float timeToCast { get; }
    //float coolDownTime { get; set; }
    //float lastUsedTime { get; set; }
    //Object aoeTarget { get; set; }



    void Execute(CharacterAttributes attributes, GameObject origin, GameObject target);
    bool isReady();
    AbilityHelper.Action GetAction();

}
