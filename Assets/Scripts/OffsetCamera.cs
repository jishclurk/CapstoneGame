using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetCamera : MonoBehaviour
{

    public GameObject followPlayer;
    private TeamManager tm;
    private Vector3 offset;

    //Simple script from Roll a Ball
    void Start()
    {
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
        followPlayer = tm.activePlayer.gameObject;
        offset = transform.position - followPlayer.transform.position;
        Debug.Log("offset:" + offset);
    }

    void LateUpdate()
    {
        if (transform.position != followPlayer.transform.position + offset)
        {
            transform.position = Vector3.Lerp(transform.position, followPlayer.transform.position + offset, 0.2f);

        }else
        {
            transform.position = followPlayer.transform.position + offset;
        }
    }

}
