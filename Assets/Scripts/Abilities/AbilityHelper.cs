using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHelper : MonoBehaviour {

    public enum Action { Basic, NoTarget, InheritTarget, AimTarget, AimAOE, HoldToUse }

    public void GrenadeThrowRoutine(CharacterAttributes attributes, GameObject origin, GameObject target, float baseDamage, Object explosion, float timeToCast, Object grenade)
    {
        Debug.Log("goal target: " + target.transform.position);
        Vector3 nadeOrigin = new Vector3(origin.transform.position.x, origin.transform.position.y + 0.8f, origin.transform.position.z + 0.1f);
        GameObject nade = Instantiate(grenade, nadeOrigin, Quaternion.identity) as GameObject;

        nade.transform.LookAt(target.transform);
        nade.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 4.9f, 0.0f);
        nade.GetComponent<Rigidbody>().angularVelocity = new Vector3(8.0f, 2.0f, 7.0f);
        StartCoroutine(MoveObject(nade.transform, target.transform, 1.0f));
        Destroy(nade, 1.0f);
        
        StartCoroutine(Explode(attributes, origin, target, baseDamage, explosion, timeToCast));
    }

    private IEnumerator Explode(CharacterAttributes attributes, GameObject origin, GameObject target, float baseDamage, Object explosion, float timeToCast)
    {
        yield return new WaitForSeconds(timeToCast); 
        float adjustedDamage = baseDamage + attributes.Strength * 2;
        Debug.Log(name + " on " + target.name + " does " + adjustedDamage + " damage.");
        AOETargetController aoeController = target.GetComponent<AOETargetController>();

        foreach (GameObject enemy in aoeController.affectedEnemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(adjustedDamage);
        }
        Object exp = Instantiate(explosion, target.transform.position, Quaternion.identity);
        Destroy(target);
        Destroy(exp, 1.0f);

    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

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
            Debug.Log("attempt target: " + target.transform.position);
            Debug.Log("current target: " + grenade.position);
            grenade.position = newPosition;

            yield return null;
        }
    }
}
