using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFieldScript : MonoBehaviour {

    public float damage = 10f;
    public float secondsBetweenDamage = 1f;

    private List<Collider> playersInCollider = new List<Collider>();


    void Start()
    {
        StartCoroutine(DamageOverTime());
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && !playersInCollider.Contains(other))
        {
            playersInCollider.Add(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && playersInCollider.Contains(other))
        {
            playersInCollider.Remove(other);
        }
    }

    IEnumerator DamageOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(secondsBetweenDamage);

            foreach (Collider playerCol in playersInCollider)
            {
                PlayerResources resources = playerCol.gameObject.GetComponent<PlayerResources>();
                if (resources != null && !resources.isDead)
                {
                    resources.TakeDamage(damage);
                }
            }
        }
    }
}
