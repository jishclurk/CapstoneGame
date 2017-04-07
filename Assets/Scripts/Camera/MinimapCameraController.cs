using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour {

    private GameObject activePlayerCharacter;
    private TeamManager tm;
    private Vector3 offset;

    //Simple script from Roll a Ball
    void Start()
    {
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
        if (tm)
        {
            activePlayerCharacter = tm.activePlayer.gameObject;
            if (activePlayerCharacter)
            {
                float offsetXValue = 0f;
                float offsetYValue = 30f;
                float offsetZValue = 0f;
                offset = new Vector3(offsetXValue, offsetYValue, offsetZValue);
                transform.position = activePlayerCharacter.transform.position + offset;
            }
            else
            {
                Debug.Log("ERROR: Minimap Camera cannot locate active player character!");
            }
        }
        else {
            Debug.Log("ERROR: Minimap Camera cannot locate Team Manager!");
        }
    }

    void LateUpdate()
    {
        if (transform.position != activePlayerCharacter.transform.position + offset)
        {
            transform.position = Vector3.Lerp(transform.position, activePlayerCharacter.transform.position + offset, 0.2f);
        }
        else
        {
            transform.position = activePlayerCharacter.transform.position + offset;
        }
    }
}