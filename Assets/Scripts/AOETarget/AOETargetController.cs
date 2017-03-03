using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LayerDefinitions;

public class AOETargetController : MonoBehaviour
{

    public HashSet<GameObject> affectedEnemies;
    public HashSet<GameObject> affectedPlayers;
    public float effectiveRange;
    public Player activePlayer;
    private Vector3 location;

    private void Awake()
    {
        affectedEnemies = new HashSet<GameObject>();
        affectedPlayers = new HashSet<GameObject>();
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
                    location = new Vector3(hit.point.x, hit.point.y + 0.3f, hit.point.z);
                    transform.position = location;
                }
                else
                {
                    location = new Vector3(hit.point.x, hit.point.y + 0.3f, hit.point.z);
                    Vector3 playerToPoint = location - activePlayer.transform.position;
                    Vector3 adjustedPosition = activePlayer.transform.position + (Vector3.Normalize(playerToPoint) * effectiveRange);
                    transform.position = adjustedPosition;
                }
                
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger && other.tag.Equals("Enemy") && !other.GetComponent<EnemyHealth>().isDead)
        {
            affectedEnemies.Add(other.gameObject);
        } else if (!other.isTrigger && other.tag.Equals("Player") && !other.GetComponent<PlayerResources>().isDead)
        {
            affectedPlayers.Add(other.gameObject);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger && other.tag.Equals("Enemy"))
        {
            affectedEnemies.Remove(other.gameObject);
        } else if (!other.isTrigger && other.tag.Equals("Player"))
        {
            affectedPlayers.Remove(other.gameObject);
        }

    }
}