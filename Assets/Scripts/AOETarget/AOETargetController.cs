using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LayerDefinitions;

public class AOETargetController : MonoBehaviour
{

    public HashSet<GameObject> affectedEnemies;
    public float effectiveRange;
    public Player activePlayer;

    private void Start()
    {
        affectedEnemies = new HashSet<GameObject>();
        effectiveRange = Mathf.Infinity;
        activePlayer = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>().activePlayer;
    }

    // Update is called once per frame
    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 200f, Layers.Floor))
        {
            if (hit.collider.CompareTag("Floor"))
            {
                if (Vector3.Distance(activePlayer.transform.position, hit.point) < effectiveRange)
                {
                    transform.position = hit.point;
                }
                else
                {
                    Vector3 playerToPoint = hit.point - activePlayer.transform.position;
                    Vector3 adjustedPosition = activePlayer.transform.position + (Vector3.Normalize(playerToPoint) * effectiveRange);
                    transform.position = adjustedPosition;
                }
                
            }
        }
    }

    //update shared watchedEnemies between co-op and ai
    private void OnTriggerEnter(Collider other)
    {
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