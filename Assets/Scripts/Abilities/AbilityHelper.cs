using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHelper : MonoBehaviour {

    public enum Action { Basic, NoTarget, InheritTarget, AimTarget, AimAOE, HoldToUse }

    public void GrenadeThrowRoutine(CharacterAttributes attributes, GameObject origin, GameObject target, float baseDamage, Object explosion, float timeToCast)
    {
        StartCoroutine(Explode(attributes, origin, target, baseDamage, explosion, timeToCast));
    }

    private IEnumerator Explode(CharacterAttributes attributes, GameObject origin, GameObject target, float baseDamage, Object explosion, float timeToCast)
    {
        yield return new WaitForSeconds(timeToCast - 0.1f);    //Wait 1 seconds
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
}
