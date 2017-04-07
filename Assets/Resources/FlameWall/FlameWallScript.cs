using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameWallScript : MonoBehaviour {

    public float damage;


    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger && other.tag.Equals("Enemy") && !other.GetComponent<EnemyHealth>().isDead)
        {
            other.GetComponent<EnemyHealth>().TakeDamage(damage);
            other.GetComponent<EnemyHealth>().TakeBurnDamage(2);
        }

    }
}
