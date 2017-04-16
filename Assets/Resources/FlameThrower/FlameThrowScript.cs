using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LayerDefinitions;

public class FlameThrowScript : MonoBehaviour {

    public HashSet<GameObject> affectedEnemies;
    //public HashSet<GameObject> affectedPlayers;
    public float effectiveRange;
    private TeamManager tm;
    public Player castedPlayer;
    public float damage;
    private ParticleSystem ps;
    private AudioSource sound;
    private Light lite;
    private bool damageOn;
    public bool dissipate;

    private Vector3 playerToPoint;

    private void Awake()
    {
        affectedEnemies = new HashSet<GameObject>();
        effectiveRange = Mathf.Infinity; //set by Ability
        damage = 0; //set by Ability
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
        ps = GetComponent<ParticleSystem>();
        sound = GetComponent<AudioSource>();
        lite = transform.FindChild("Point light").GetComponent<Light>();
        damageOn = false;
        dissipate = false;
    }

    // Use this for initialization
    void Start() {
        ps.Stop();
        lite.enabled = false;
        InvokeRepeating("tickDamage", 0.0f, 0.2f);
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (dissipate)
        {
            Debug.Log("DONE!");
            ps.Stop();
            sound.volume = sound.volume - 0.03f;
            lite.enabled = false;
            return;
        }

        transform.position = castedPlayer.gunbarrel.position;
        transform.rotation = castedPlayer.transform.rotation;


        if (castedPlayer == tm.activePlayer)
        {
            //player controlled code
            if (Input.GetButton("Fire1"))
            {
                if (!ps.isPlaying)
                {
                    ps.Play();
                    sound.Play();
                    lite.enabled = true;
                }

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 200f, Layers.Floor))
                {
                    if (hit.collider.CompareTag("Floor"))
                    {
                        castedPlayer.transform.LookAt(new Vector3(hit.point.x, castedPlayer.transform.position.y, hit.point.z));
                        playerToPoint = Vector3.Normalize(hit.point - castedPlayer.transform.position) * effectiveRange;
                        damageOn = true;

                    }
                }
            } else
            {
                if (ps.isPlaying)
                {
                    sound.Stop();
                    ps.Stop();
                    lite.enabled = false;
                }
                damageOn = false;
            }

        }
        else
        {
            //ai controlled code
            if (castedPlayer.strategy.aiScript.targetedEnemy != null)
            {
                if (!ps.isPlaying)
                {
                    ps.Play();
                    sound.Play();
                    lite.enabled = true;
                }
                playerToPoint = Vector3.Normalize(castedPlayer.strategy.aiScript.targetedEnemy.position - castedPlayer.transform.position) * effectiveRange;
                damageOn = true;

            } else
            {
                if (ps.isPlaying)
                {
                    ps.Stop();
                    sound.Stop();
                    lite.enabled = false;
                }
                damageOn = false;
            }
        }
    }

    private void tickDamage()
    {
        if (damageOn)
        {
            HashSet<EnemyHealth> ehSet = new HashSet<EnemyHealth>();
            foreach (GameObject enemy in castedPlayer.watchedEnemies)
            {
                ehSet.Add(enemy.GetComponent<EnemyHealth>());
            }
            foreach (EnemyHealth eh in ehSet)
            {
                Vector3 playerToTarget = eh.transform.position - castedPlayer.transform.position;
                if (playerToTarget.magnitude <= effectiveRange && Mathf.Abs(Vector3.Angle(playerToPoint, playerToTarget)) < 30.0f)
                {
                    if (eh != null)
                    {
                        eh.TakeDamage(damage);
                        if (eh.isDead)
                        {
                            //on kill, remove from both team manager visible enemies and all local watchedenemies
                            castedPlayer.strategy.aiScript.targetedEnemy = null;
                        }
                    }
                }

            }
        }


    }

}
