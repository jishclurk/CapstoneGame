using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SniperShot : IBasic, IAbility {

    public string name { get; set; }
    public string description { get; set; }
    public int id { get; private set; }
    public Image image { get; private set; }
    public float effectiveRange { get; set; }

    public float baseDamage { get; set; }
    public float fireRate { get; set; }
    private float lastUsedTime { get; set; }

    public int StrengthRequired { get; private set; }
    public int StaminaRequired { get; private set; }
    public int IntelligenceRequired { get; private set; }

    private Object bullet = Resources.Load("SniperShot/YellowSniperObj");

    public SniperShot()
    {
        StrengthRequired = 0;
        StaminaRequired = 0;
        IntelligenceRequired = 15;
        image = Resources.Load("Abilities/Sniper", typeof(Image)) as Image;
        id = 4;
        name = "Sniper Rifle";
        description = "*Basic Attack* A high damage, long range Sniper Rifle. ";
        effectiveRange = 11.0f;
        baseDamage = 22.0f;
        fireRate = 0.9f;
        lastUsedTime = 0.0f;

    }

    public void Execute(Player player, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        lastUsedTime = Time.time;
        float adjustedDamage = baseDamage + (baseDamage * player.attributes.TotalStrength * 0.04f);
        target.GetComponent<EnemyHealth>().TakeDamage(adjustedDamage);

        GameObject project = Object.Instantiate(bullet, player.gunbarrel.position, Quaternion.identity) as GameObject;
        project.GetComponent<SniperProjectileScript>().destination = new Vector3(target.transform.position.x, (player.gunbarrel.position.y + target.transform.position.y)/2, target.transform.position.z);
        project.GetComponent<SniperProjectileScript>().targetedEnemy = target;
        //useEnergy not required
    }

    public bool isReady()
    {
        return Time.time > lastUsedTime + fireRate;
    }

    public AbilityHelper.Action GetAction()
    {
        return AbilityHelper.Action.Basic;
    }

    public AbilityHelper.CoopAction GetCoopAction()
    {
        return AbilityHelper.CoopAction.Basic;
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

        return description + "Requires: " + strReq + intReq + stmReq + "Damage: " + (baseDamage + (baseDamage * p.attributes.TotalStrength * 0.04f)) + " per shot";
    }
}
