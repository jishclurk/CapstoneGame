using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Invigorate : ISpecial, IAbility {

    public string name { get; set; }
    public string description { get; set; }
    public int id { get; private set; }
    public Image image { get; private set; }
    public float effectiveRange { get; set; }

    public float baseDamage { get; set; }
    public float energyRequired { get; set; }
    public float coolDownTime { get; set; }
    public float timeToCast { get; set; }
    public Object aoeTarget { get; set; }

    private float lastUsedTime { get; set; }

    public int StrengthRequired { get; private set; }
    public int StaminaRequired { get; private set; }
    public int IntelligenceRequired { get; private set; }

    private Object sound;

    public Invigorate()
    {
        StrengthRequired = 10;
        StaminaRequired = 0;
        IntelligenceRequired = 10;
        image = Resources.Load("Abilities/InvigorateIcon", typeof(Image)) as Image;
        id = 10;
        name = "Invigorate";
        effectiveRange = 0.0f;
        baseDamage = 0.0f;
        timeToCast = 0.0f;
        coolDownTime = 30.0f;
        lastUsedTime = -Mathf.Infinity;
        energyRequired = 0.0f;
        aoeTarget = null;
        description = "Resets all ability cooldown timers to zero seconds. ";
        sound = Resources.Load("Invigorate/InvigorateSound");
    }

    public void Execute(Player player, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        foreach(ISpecial ability in player.abilities.abilityArray)
        {
            ability.setAsReady();
        }
        lastUsedTime = Time.time;
        player.resources.UseEnergy(energyRequired);
        GameObject emptySound = GameObject.Instantiate(sound) as GameObject;
        GameObject.Destroy(emptySound, 2.0f);
    }

    public void updatePassiveBonuses(CharacterAttributes attributes)
    {
    }

    public bool isReady()
    {
        return Time.time > lastUsedTime + coolDownTime;
    }

    public void setAsReady()
    {
        //cannot set itself as ready
    }

    public bool EvaluateCoopUse(Player player, Transform targetedEnemy, TeamManager tm)
    {
        int count = 0;
        foreach (ISpecial spec in player.abilities.abilityArray)
        {
            if (!spec.isReady())
            {
                count++;
            }
        }
        return count > 1;
    }

    public float RemainingTime()
    {
        return lastUsedTime + coolDownTime - Time.time;
    }

    public AbilityHelper.Action GetAction()
    {
        return AbilityHelper.Action.Equip;
    }

    public AbilityHelper.CoopAction GetCoopAction()
    {
        return AbilityHelper.CoopAction.InstantHeal;
    }

    public string GetHoverDescription(Player p)
    {
        string strReq = "";
        string intReq = "";
        string stmReq = "";
        if (p.attributes.Strength < StrengthRequired)
        {
            strReq = StrengthRequired + " " + "STR. ";
        }
        if (p.attributes.Intelligence < IntelligenceRequired)
        {
            intReq = IntelligenceRequired + " " + "INT. ";
        }
        if (p.attributes.Stamina < StaminaRequired)
        {
            stmReq = StaminaRequired + " " + "STM. ";
        }
        string requires = "Requires: ";
        if (strReq.Length == 0 && intReq.Length == 0 && stmReq.Length == 0)
        {
            requires = " ";
        }

        return description + requires + strReq + intReq + stmReq + "Cooldown: " + coolDownTime + " seconds.";
    }
}
