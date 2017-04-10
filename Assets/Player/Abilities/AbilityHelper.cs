using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AbilityHelper : MonoBehaviour {

    public enum Action { Basic, Equip, Target, TargetFriend, AOE, Passive }
    public enum CoopAction { Basic, InstantHeal, Equip, TargetHeal, TargetHurt, AOEHeal, AOEHurt, Passive }


    // GRENADE THROW //

    public void GrenadeThrowRoutine(CharacterAttributes attributes, GameObject origin, GameObject target, float damage, Object explosion, float timeToCast, Object grenade)
    {
        Vector3 nadeOrigin = new Vector3(origin.transform.position.x, origin.transform.position.y + 0.8f, origin.transform.position.z + 0.1f);
        GameObject nade = Instantiate(grenade, nadeOrigin, Quaternion.identity) as GameObject;

        nade.transform.LookAt(target.transform);
        nade.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 4.9f, 0.0f);
        nade.GetComponent<Rigidbody>().angularVelocity = new Vector3(8.0f, 2.0f, 7.0f);
        StartCoroutine(MoveObject(nade.transform, target.transform, 0.95f));
        Destroy(nade, 1.0f);
        
        StartCoroutine(Explode(attributes, origin, target, damage, explosion, timeToCast));
    }

    private IEnumerator Explode(CharacterAttributes attributes, GameObject origin, GameObject target, float damage, Object explosion, float timeToCast)
    {
        yield return new WaitForSeconds(timeToCast); 
        AOETargetController aoeController = target.GetComponent<AOETargetController>();

        foreach (GameObject enemy in aoeController.affectedEnemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
        Object exp = Instantiate(explosion, target.transform.position, Quaternion.identity);
        Destroy(target, 0.05f);
        Destroy(exp, 1.0f);
    }

    // BIO GRENADE //

    public void BioGrenadeRoutine(CharacterAttributes attributes, GameObject origin, GameObject target, float baseHeal, float healRate, float healLength, float timeToCast, Object bubble)
    {
        Vector3 bubbleOrigin = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
        GameObject shield = Instantiate(bubble, bubbleOrigin, Quaternion.identity) as GameObject;
        HealBubbleScript hb = shield.GetComponent<HealBubbleScript>();
        hb.healHP = baseHeal;
        hb.healRate = healRate;
        hb.healLength = healLength;
        hb.target = target;
        Destroy(shield, healLength);
        Destroy(target, healLength);

    }

    // FLAME THROWER //

    public void FlameThrowerRoutine(Player player, GameObject origin, GameObject target, Object flame, float baseDamage, float effectiveRange, float length)
    {
        GameObject worldFlame = Instantiate(flame, player.gunbarrel.position, Quaternion.identity) as GameObject;
        worldFlame.transform.parent = player.transform;
        FlameThrowScript ft = worldFlame.GetComponent<FlameThrowScript>();
        ft.castedPlayer = player;
        ft.effectiveRange = effectiveRange;
        ft.damage = baseDamage;
        Destroy(worldFlame, 5.0f);

    }

    // SHIELD BOOSTER //
    public void ShieldBoosterRoutine(CharacterAttributes attributes, GameObject target, float length, Object booster)
    {
        AOETargetController aoeController = target.GetComponent<AOETargetController>();
        GameObject[] affectedPlayersCopy = new GameObject[aoeController.affectedPlayers.Count];
        aoeController.affectedPlayers.CopyTo(affectedPlayersCopy);
        StartCoroutine(ShieldBoosterEffect(affectedPlayersCopy, length, booster));
        Destroy(target);
    }

    IEnumerator ShieldBoosterEffect(GameObject[] affectedPlayers, float length, Object booster)
    {
        foreach (GameObject player in affectedPlayers)
        {
            player.GetComponent<CharacterAttributes>().PassiveStamina += 50;
            GameObject gb = Object.Instantiate(booster, player.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
            gb.GetComponent<StayWithPlayer>().player = player.transform;
            Destroy(gb, length);
        }
        yield return new WaitForSeconds(length);
        foreach (GameObject player in affectedPlayers)
        {
            player.GetComponent<CharacterAttributes>().PassiveStamina -= 50;
        }
    }

    // SENTRY TURRET //
    public void SentryTurretRoutine(Player player, GameObject target, float length, Object turret)
    {
        GameObject sentry = Instantiate(turret, target.transform.position, Quaternion.identity) as GameObject;
        sentry.transform.LookAt(player.transform);
        sentry.transform.Rotate(0, 90, 0);
        sentry.GetComponent<SentryScript>().castedPlayer = player;
        Destroy(sentry, length);
        Destroy(target);
    }

    // STIMPAK //
    public void StimpakRoutine(CharacterAttributes attributes, float length)
    {
        StartCoroutine(StimpakEffect(attributes, length));
        attributes.PassiveStrength += 10;
    }

    IEnumerator StimpakEffect(CharacterAttributes attributes, float length)
    {
        yield return new WaitForSeconds(length);
        attributes.PassiveStrength -= 10;
    }

    // SHOCKWAVE //
    public void ShockwaveRoutine(Transform playerOrigin, GameObject target, Object blast, float damage, float stunLength)
    {
        AOETargetController aoeController = target.GetComponent<AOETargetController>();
        GameObject[] affectedEnemiesCopy = new GameObject[aoeController.affectedEnemies.Count];
        aoeController.affectedEnemies.CopyTo(affectedEnemiesCopy);
        Instantiate(blast, playerOrigin.position, Quaternion.Euler(-90, 0, 0));
        foreach (GameObject enemy in affectedEnemiesCopy)
        {
            Vector3 nextEnemyPos = (Vector3.Normalize(enemy.transform.position - playerOrigin.position) * 9.0f) + playerOrigin.position;
            StartCoroutine(ShockwaveEnemy(enemy.transform, nextEnemyPos, 0.5f));
            enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
        Destroy(target);
    }

    IEnumerator ShockwaveEnemy(Transform enemy, Vector3 target, float overTime)
    {
        yield return new WaitForFixedUpdate();
        float startTime = Time.time;
        float initialZ = enemy.position.z;
        float initialX = enemy.position.x;
        NavMeshAgent nav = enemy.GetComponent<NavMeshAgent>();
        while (Time.time < startTime + overTime)
        {
            Vector3 newPosition = enemy.position;
            newPosition.z = Mathf.Lerp(initialZ, target.z, (Time.time - startTime) / overTime);
            newPosition.x = Mathf.Lerp(initialX, target.x, (Time.time - startTime) / overTime);
            nav.Warp(newPosition);

            yield return null;
        }
        enemy.GetComponent<EnemyHealth>().TakeStunDamage(1);

    }

    // FLAME WALL //
    public void FlameWallRoutine(Player player, GameObject target, float damage, float effectLength, Object wall)
    {

        GameObject flameWall = Instantiate(wall, player.transform.position, Quaternion.identity) as GameObject;
        flameWall.GetComponent<FlameWallScript>().damage = damage;
        //set flamewall stuff

        flameWall.transform.LookAt(target.transform);
        StartCoroutine(MoveObject(flameWall.transform, target.transform, 0.3f));
        Destroy(target, 0.35f);
        Destroy(flameWall, effectLength);
    }




    //CO-OP HELPER METHODS//
    public void CoopExecuteAOE(Player player, GameObject origin, GameObject aoeTarget, ISpecial ability)
    {
        StartCoroutine(ExecuteAOE(player, origin, aoeTarget, ability));

    }
    private IEnumerator ExecuteAOE(Player player, GameObject origin, GameObject aoeTarget, ISpecial ability)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        ability.Execute(player, origin, aoeTarget);
    }



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Move object in x, z plane
    IEnumerator MoveObject(Transform grenade, Transform target, float overTime)
    {
        float startTime = Time.time;
        float initialZ = grenade.position.z;
        float initialX = grenade.position.x;
        while (Time.time < startTime + overTime)
        {
            Vector3 newPosition = grenade.position;
            newPosition.z = Mathf.Lerp(initialZ, target.position.z, (Time.time - startTime) / overTime);
            newPosition.x = Mathf.Lerp(initialX, target.position.x, (Time.time - startTime) / overTime);
            grenade.position = newPosition;

            yield return null;
        }
    }



    //

}
