using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetCamera : MonoBehaviour
{

    public GameObject followPlayer;
    private TeamManager tm;
    private Vector3 offset;

    //Simple script from Roll a Ball
    void Awake()
    {
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
        followPlayer = GameObject.FindWithTag("Player"); //Find temporary player until TeamManager updates this
        offset = transform.position - followPlayer.transform.position;
    }

    void LateUpdate()
    {
        transform.position = followPlayer.transform.position + offset;
    }

}
