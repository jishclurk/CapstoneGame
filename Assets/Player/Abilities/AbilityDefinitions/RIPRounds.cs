using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RIPRounds : ISpecial, IAbility {

    public string name { get; set; }
    public string useType { get; set; }
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

    public RIPRounds()
    {
        StrengthRequired = 20;
        StaminaRequired = 0;
        IntelligenceRequired = 0;
        image = Resources.Load("Abilities/RIPRoundsIcon", typeof(Image)) as Image;
        id = 14;
        name = "R.I.P. Rounds";
        useType = "Passive";
        effectiveRange = 0.0f;
        baseDamage = 0.0f;
        timeToCast = 0.0f;
        coolDownTime = 0.0f;
        lastUsedTime = -Mathf.Infinity;
        energyRequired = 0.0f;
        aoeTarget = null;
        description = "Doubles the user's strength to deal more damage with all abilities.\n";
    }

    public void Execute(Player player, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        //no code here, this is a passive
    }

    public void updatePassiveBonuses(CharacterAttributes attributes)
    {
        attributes.PassiveStrength += attributes.Strength;
    }

    public bool isReady()
    {
        return true;
    }

    public void setAsReady()
    {
        
    }

    public bool EvaluateCoopUse(Player player, Transform targetedEnemy, TeamManager tm)
    {
        bool use = false;
        if(targetedEnemy != null)
        {
            EnemyHealth eh = targetedEnemy.GetComponent<EnemyHealth>();
            use = eh.maxHealth - eh.currentHealth > baseDamage/2;
        }
        return use;
    }

    public float RemainingTime()
    {
        return lastUsedTime + coolDownTime - Time.time;
    }

    public AbilityHelper.Action GetAction()
    {
        return AbilityHelper.Action.Passive;
    }

    public AbilityHelper.CoopAction GetCoopAction()
    {
        return AbilityHelper.CoopAction.Passive;
    }

    public string GetHoverDescription(Player p)
    {
        string strReq = "";
        string intReq = "";
        string stmReq = "";
        if (StrengthRequired > 0)
        {
            strReq = StrengthRequired + " " + "STR. ";
        }
        if (IntelligenceRequired > 0)
        {
            intReq = IntelligenceRequired + " " + "INT. ";
        }
        if (StaminaRequired > 0)
        {
            stmReq = StaminaRequired + " " + "STM. ";
        }
        string requires = "Requires: ";
        if (strReq.Length == 0 && intReq.Length == 0 && stmReq.Length == 0)
        {
            requires = " ";
        }

        return description + requires + strReq + intReq + stmReq + "\nCooldown: " + coolDownTime + " seconds.";
    }
}
