using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RifleShot : IAbility {

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
    private Object projectile;

    public RifleShot()
    {
        image = (Image)AssetDatabase.LoadAssetAtPath("Assets/Images/Abilities/Pistol.prefab", typeof(Image));
        id = 8;
        name = "Rifle Shot";
        effectiveRange = 9.0f;
        baseDamage = 5.0f;
        fireRate = 0.4f;
        isbasicAttack = true;
        timeToCast = 0.0f;
        coolDownTime = 0.0f;
        lastUsedTime = 0.0f;
        requiresTarget = true;
        energyRequired = 0.0f;
        requiresAim = false;
        aoeTarget = null;
        projectile = Resources.Load("RifleShot/YellowPlasmaShot");

    }

    public void Execute(CharacterAttributes attributes, GameObject origin, GameObject target, Transform fxSpawn)
    {
        lastUsedTime = Time.time;

        float adjustedDamage = baseDamage + attributes.Strength * 0.1f;
        Debug.Log(name + " on " + target.name + " does " + adjustedDamage + " damage.");
        target.GetComponent<EnemyHealth>().TakeDamage(adjustedDamage);

        Vector3 playerToTarget = target.transform.position - fxSpawn.position;
        RaycastHit hit;
        if (Physics.Raycast(fxSpawn.position, playerToTarget, out hit, 100f, LayerDefinitions.Layers.Enemy))
        {
            GameObject project = Object.Instantiate(projectile, fxSpawn.position, Quaternion.identity) as GameObject;
            project.transform.LookAt(target.transform.position);
        }

        //useEnergy not required
    }

    public bool isReady()
    {
        return Time.time > lastUsedTime + fireRate;
    }
}
