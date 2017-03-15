﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LayerDefinitions;

public class FlameThrowScript : MonoBehaviour {

    public HashSet<GameObject> affectedEnemies;
    //public HashSet<GameObject> affectedPlayers;
    public float effectiveRange;
    public TeamManager tm;
    public Player castedPlayer;
    private ParticleSystem ps;
    private Light lite;

    private void Awake()
    {
        affectedEnemies = new HashSet<GameObject>();
        effectiveRange = Mathf.Infinity;
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
        ps = GetComponent<ParticleSystem>();
        lite = transform.FindChild("Point light").GetComponent<Light>();
    }

    // Use this for initialization
    void Start () {
        ps.Stop();
        lite.enabled = false;
		
	}
	
	// Update is called once per frame
	void Update () {
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
                    lite.enabled = true;
                }
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 200f, Layers.Floor))
                {
                    if (hit.collider.CompareTag("Floor"))
                    {
                        castedPlayer.transform.LookAt(new Vector3(hit.point.x, castedPlayer.transform.position.y, hit.point.z));
                        Vector3 playerToPoint = Vector3.Normalize(hit.point - castedPlayer.transform.position) * effectiveRange;

                        foreach (GameObject enemy in castedPlayer.watchedEnemies)
                        {
                            Vector3 playerToTarget = enemy.transform.position - castedPlayer.transform.position;
                            if (playerToTarget.magnitude <= effectiveRange && Mathf.Abs(Vector3.Angle(playerToPoint, playerToTarget)) < 30.0f)
                            {
                                enemy.GetComponent<EnemyHealth>().TakeDamage(1);
                            }

                        }

                    }
                }
            } else
            {
                if (ps.isPlaying)
                {
                    ps.Stop();
                    lite.enabled = false;
                }
            }

        }
        else
        {
            //ai controlled code
            if(castedPlayer.strategy.aiScript.targetedEnemy != null)
            {
                if (!ps.isPlaying)
                {
                    ps.Play();
                    lite.enabled = true;
                }
                
                Vector3 playerToPoint = Vector3.Normalize(castedPlayer.strategy.aiScript.targetedEnemy.position - castedPlayer.transform.position) * effectiveRange;
                foreach (GameObject enemy in castedPlayer.watchedEnemies)
                {
                    Vector3 playerToTarget = enemy.transform.position - castedPlayer.transform.position;
                    if (playerToTarget.magnitude <= effectiveRange && Mathf.Abs(Vector3.Angle(playerToPoint, playerToTarget)) < 30.0f)
                    {
                        enemy.GetComponent<EnemyHealth>().TakeDamage(1);
                    }

                }
            } else
            {
                if (ps.isPlaying)
                {
                    ps.Stop();
                    lite.enabled = false;
                }
            }
            



        }
    }
}
