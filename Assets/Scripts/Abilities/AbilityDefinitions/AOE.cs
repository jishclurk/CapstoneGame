using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AOE : IAbility {

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
    private float nextFire;
    public int id { get; private set; }
    public Image image { get; private set; }

    public AOE()
    {
        image = (Image)AssetDatabase.LoadAssetAtPath("Assets/Images/Abilities/Grenade.prefab", typeof(Image));
        id = 0;
        name = "Area of Effect";
        effectiveRange = 10.0f;
        baseDamage = 25.0f;
        fireRate = 0.0f;
        isbasicAttack = false;
        timeToCast = 0.0f;
        coolDownTime = 1.0f;
        lastUsedTime = -Mathf.Infinity;
        requiresTarget = true;
        energyRequired = 10.0f;
        requiresAim = true;
        aoeTarget = Resources.Load("3x3RedAuraTarget");
    }

    public void Execute(CharacterAttributes attributes, GameObject origin, GameObject target, Transform fxSpawn)
    {
        lastUsedTime = Time.time;
        float adjustedDamage = baseDamage + attributes.Strength * 2;
        Debug.Log(name + " on " + target.name + " does " + adjustedDamage + " damage.");
        AOETargetController aoeController = target.GetComponent<AOETargetController>();

        foreach (GameObject enemy in aoeController.affectedEnemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(adjustedDamage);
        }
        origin.GetComponent<PlayerResources>().UseEnergy(energyRequired);
        
    }


    public bool isReady()
    {
        return Time.time > lastUsedTime + coolDownTime;
    }
}
