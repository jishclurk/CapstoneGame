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
    public bool isPlayerCalled;
    private Vector3 location;
    private ParticleSystem ps;


    private float lastTime;


    private void Awake()
    {
        affectedEnemies = new HashSet<GameObject>();
        affectedPlayers = new HashSet<GameObject>();
        effectiveRange = Mathf.Infinity;
        activePlayer = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>().activePlayer;
        isPlayerCalled = true;
        ps = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        lastTime = Time.realtimeSinceStartup;
        if (isPlayerCalled)
        {
            StartCoroutine(UpdateAOEPosition());
        }
       
    }

    //this is weird, but I can explain (NC)
    //We only want this to occur when *players* use an AOETarget
    //if a co-op player uses an AOE, we don't want Mouse movement at all, so this serves as a way I can "turn off" the update function
    IEnumerator UpdateAOEPosition()
    {
        while (enabled) //enabled here means while AOETargetController script (this one) is enabled
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
                        float holdXRot = transform.eulerAngles.x;
                        transform.LookAt(activePlayer.transform.position);
                        transform.rotation = Quaternion.Euler(holdXRot, transform.eulerAngles.y, transform.eulerAngles.z);
                    }
                    else
                    {
                        location = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                        Vector3 playerToPoint = location - activePlayer.transform.position;
                        Vector3 adjustedPosition = activePlayer.transform.position + (Vector3.Normalize(playerToPoint) * effectiveRange) + new Vector3(0, 0.3f, 0);
                        transform.position = adjustedPosition;
                        float holdXRot = transform.eulerAngles.x;
                        transform.LookAt(activePlayer.transform.position);
                        transform.rotation = Quaternion.Euler(holdXRot, transform.eulerAngles.y, transform.eulerAngles.z);
                    }

                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.realtimeSinceStartup - lastTime;
        ps.Simulate(deltaTime, true, false); //last must be false!!
        lastTime = Time.realtimeSinceStartup;
    }

    public void DisableTracking()
    {
        StopAllCoroutines();
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