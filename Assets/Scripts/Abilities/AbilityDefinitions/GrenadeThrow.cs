using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeThrow : ISpecial, IAbility {

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

    private Object explosion;
    private Object grenade;
    private GameObject abilityObj;
    

    public GrenadeThrow()
    {
        image = GameObject.Instantiate(Resources.Load("Abilities/Grenade", typeof(Image))) as Image;
        id = 0;
        name = "Grenade Throw";
        effectiveRange = 10.0f;
        baseDamage = 25.0f;
        fireRate = 0.0f;
        isbasicAttack = false;
        timeToCast = 1.0f;
        coolDownTime = 5.0f;
        lastUsedTime = -Mathf.Infinity;
        requiresTarget = true;
        energyRequired = 25.0f;
        requiresAim = true;
        aoeTarget = Resources.Load("GrenadeThrow/3x3GreenAuraTarget");
        explosion = Resources.Load("GrenadeThrow/explosion");
        abilityObj = GameObject.FindWithTag("AbilityHelper");
        grenade = Resources.Load("GrenadeThrow/nade");
    }

    public void Execute(CharacterAttributes attributes, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {  
        abilityObj.GetComponent<AbilityHelper>().GrenadeThrowRoutine(attributes, origin, target, baseDamage, explosion, timeToCast, grenade);
        /*float adjustedDamage = baseDamage + attributes.Strength * 2;
        Debug.Log(name + " on " + target.name + " does " + adjustedDamage + " damage.");
        AOETargetController aoeController = target.GetComponent<AOETargetController>();

        foreach (GameObject enemy in aoeController.affectedEnemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(adjustedDamage);
        }
        
        Object exp = Object.Instantiate(explosion, target.transform.position, Quaternion.identity);
        Object.Destroy(exp, 1.0f);*/

        lastUsedTime = Time.time;
        
        origin.GetComponent<PlayerResources>().UseEnergy(energyRequired);
        
    }

    /*
    private IEnumerator Explode(CharacterAttributes attributes, GameObject origin, GameObject target)
    {
        yield return new WaitForSeconds(1.0f);    //Wait 1 seconds
        float adjustedDamage = baseDamage + attributes.Strength * 2;
        Debug.Log(name + " on " + target.name + " does " + adjustedDamage + " damage.");
        AOETargetController aoeController = target.GetComponent<AOETargetController>();

        foreach (GameObject enemy in aoeController.affectedEnemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(adjustedDamage);
        }
        Object exp = Instantiate(explosion, target.transform.position, Quaternion.identity);
        Destroy(exp, 1.0f);

    }*/


    public bool isReady()
    {
        return Time.time > lastUsedTime + coolDownTime;
    }

    public AbilityHelper.Action GetAction()
    {
        return AbilityHelper.Action.AimAOE;
    }
}
