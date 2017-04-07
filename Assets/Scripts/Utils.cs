using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Utils  {


    public static Dictionary<int, IAbility> AbilityIDs = new Dictionary<int, IAbility>
    {
        {1,  new RifleShot()}, {2, new SMGShot()}, {3, new ShotgunShot() }, {4, new SniperShot() }, {5, new BioGrenade() },
        { 6, new ChainLightning() }, {7, new Flamethrower() }, {8, new FlameWall() }, {9, new GrenadeThrow() }, {10, new Invigorate() },
        { 11, new LeechDart() }, {12, new MedKit() }, {13, new Revive() }, {14, new RIPRounds() }, {15, new Shockwave() },
        { 16, new ShieldBooster() }, {17, new Shockwave() }, {18, new Stimpak() }, {19, new Zap() }
    };

}
