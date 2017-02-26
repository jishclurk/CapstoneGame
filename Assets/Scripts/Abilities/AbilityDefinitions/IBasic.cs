using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBasic : IAbility {

    float effectiveRange { get; }
    float baseDamage { get;  }
    float fireRate { get;  }

}
