using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SMGShot : IBasic, IAbility {

    public string name { get; set; }
    public string useType { get; set; }
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

    private Object bullet = Resources.Load("SMGShot/SMGProjectile");

    public SMGShot()
    {
        StrengthRequired = 15;
        StaminaRequired = 0;
        IntelligenceRequired = 0;
        image = Resources.Load("Abilities/SMG", typeof(Image)) as Image;
        id = 2;
        name = "SMG";
        useType = "Basic Attack";
        description = "A high fire rate Sub Machine Gun.\n";
        effectiveRange = 8.0f;
        baseDamage = 2.0f;
        fireRate = 0.11f;
        lastUsedTime = 0.0f;

    }

    public void Execute(Player player, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        lastUsedTime = Time.time;
        float adjustedDamage = baseDamage + (baseDamage * player.attributes.TotalStrength * 0.08f);
        target.GetComponent<EnemyHealth>().TakeDamage(adjustedDamage);

        GameObject project = Object.Instantiate(bullet, player.gunbarrel.position, Quaternion.identity) as GameObject;
        project.GetComponent<SMGProjectileScript>().destination = new Vector3(target.transform.position.x, (player.gunbarrel.position.y + target.transform.position.y)/2, target.transform.position.z);
        project.GetComponent<SMGProjectileScript>().targetedEnemy = target;
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
        string requires = "Requires: ";
        if (strReq.Length == 0 && intReq.Length == 0 && stmReq.Length == 0)
        {
            requires = " ";
        }

        return description + requires + strReq + intReq + stmReq + "\nDamage: " + (baseDamage + (baseDamage * p.attributes.TotalStrength * 0.08f)) + " per shot";
    }
}
