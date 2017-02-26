using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SelfHeal : ISpecial, IAbility
{

    public string name { get; set; }
    public float effectiveRange { get; set; }
    public float baseDamage { get; set; }
    public float fireRate { get; set; }
    public bool isbasicAttack { get; set; }
    public float energyRequired { get; set; }
    public float timeToCast { get; set; }
    public float coolDownTime { get; set; }
    public float lastUsedTime { get; set; }
    public bool requiresTarget { get; set; }
    public bool requiresAim { get; set; }
    public Object aoeTarget { get; set; }
    public int id { get; private set; }
    public Image image { get; private set; }

    public SelfHeal()
    {
        //Image imagePrefab = (Image)AssetDatabase.LoadAssetAtPath("Assets/Images/Abilities/Heal.prefab", typeof(Image));
        //image = GameObject.Instantiate(imagePrefab) as Image;
        id = 3;
        name = "Heal";
        effectiveRange = 5.0f;
        baseDamage = 20.0f;
        fireRate = 0.0f;
        isbasicAttack = false;
        timeToCast = 0.0f;
        coolDownTime = 4.0f;
        lastUsedTime = -Mathf.Infinity;
        requiresTarget = false;
        energyRequired = 20.0f;
        requiresAim = false;
        aoeTarget = null;
    }

    public void Execute(CharacterAttributes attributes, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        lastUsedTime = Time.time;
        float adjustedDamage = baseDamage + attributes.Intelligence;
        Debug.Log(name + " on " + target.name + " heals " + adjustedDamage + ".");
        target.GetComponent<PlayerResources>().Heal(adjustedDamage);
        origin.GetComponent<PlayerResources>().UseEnergy(energyRequired);
    }

    public bool isReady()
    {
        return Time.time > lastUsedTime + coolDownTime;
    }

    public AbilityHelper.Action GetAction()
    {
        return AbilityHelper.Action.NoTarget;
    }
}
