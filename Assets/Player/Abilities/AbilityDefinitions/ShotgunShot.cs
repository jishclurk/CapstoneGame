using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotgunShot : IBasic, IAbility {

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

    private Object bullet = Resources.Load("ShotgunShot/ShotgunProjectile");

    public ShotgunShot()
    {
        StrengthRequired = 0;
        StaminaRequired = 15;
        IntelligenceRequired = 0;
        image = Resources.Load("Abilities/Shotgun", typeof(Image)) as Image;
        id = 3;
        name = "Shotgun Shot";
        effectiveRange = 8.0f;
        baseDamage = 5.0f;
        fireRate = 0.8f;
        lastUsedTime = 0.0f;

    }

    public void Execute(Player player, GameObject origin, GameObject target) //Likely to be replaced with Character or Entity?
    {
        lastUsedTime = Time.time;
        Vector3 targetDest = new Vector3(target.transform.position.x, player.gunbarrel.position.y, target.transform.position.z);

        float adjustedDamage = baseDamage + player.attributes.Strength * 0.5f;
        
        for(float shellRot = -30.0f; shellRot <30.1f; shellRot += 15.0f)
        {
            GameObject project = Object.Instantiate(bullet, player.gunbarrel.position, Quaternion.identity) as GameObject;
            project.GetComponent<ShotgunProjectileScript>().destination = targetDest;
            project.GetComponent<ShotgunProjectileScript>().rot = shellRot;
            project.GetComponent<ShotgunProjectileScript>().damage = adjustedDamage;
        }


        /*Vector3 playerToPoint = Vector3.Normalize(target.transform.position - player.transform.position) * effectiveRange;
        HashSet<EnemyHealth> ehSet = new HashSet<EnemyHealth>();
        foreach (GameObject enemy in player.watchedEnemies)
        {
            ehSet.Add(enemy.GetComponent<EnemyHealth>());
        }
        foreach (EnemyHealth eh in ehSet)
        {
            Vector3 playerToTarget = eh.transform.position - player.transform.position;
            if (playerToTarget.magnitude <= effectiveRange && Mathf.Abs(Vector3.Angle(playerToPoint, playerToTarget)) < 30.0f)
            {
                if (eh != null)
                {
                    eh.TakeDamage(adjustedDamage);
                }
            }

        }*/

        //useEnergy not required*/
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
}
