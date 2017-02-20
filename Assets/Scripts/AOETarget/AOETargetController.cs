using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOETargetController : MonoBehaviour
{

    public HashSet<GameObject> affectedEnemies;

    private void Start()
    {
        affectedEnemies = new HashSet<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log("AffectedEnemies: " + affectedEnemies.Count);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 200))
        {
            if (hit.collider.CompareTag("Floor"))
            {
                transform.position = hit.point;
            }
        }
    }

    //update shared watchedEnemies between co-op and ai
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("entered?" + other.tag);
        if (!other.isTrigger && other.tag.Equals("Enemy") && !other.GetComponent<EnemyHealth>().isDead)
        {
            affectedEnemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger && other.tag.Equals("Enemy"))
        {
            affectedEnemies.Remove(other.gameObject);
        }
    }
}