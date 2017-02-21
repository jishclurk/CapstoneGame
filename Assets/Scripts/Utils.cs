using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Utils  {

    public static Dictionary<int, int> CheckPointsPerLevel = new Dictionary<int, int>() { { 1, 1 } };

    public static Dictionary<int, IAbility> AbilityIDs = new Dictionary<int, IAbility>
    {
        {0, new AOE()}, {1,  new EmptyAbility()}, {2, new PistolShot()}, {3, new SelfHeal() }, {4, new Zap() }
    };

}
