using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour {

    private GameObject activePlayerCharacter;
    private TeamManager tm;
    private Vector3 offset;
    private float offsetXValue;
    private float offsetYValue;
    private float offsetZValue;
    private float minimapSizeSpeed;
    private float minimapSizeMin;
    private float minimapSizeMax;
    private Camera minimapCamera;

    //Simple script from Roll a Ball
    void Start()
    {
        minimapCamera = gameObject.GetComponent<Camera>();
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
        if (tm)
        {
            activePlayerCharacter = tm.activePlayer.gameObject;
            if (activePlayerCharacter)
            {
                offsetXValue = 0f;
                offsetYValue = 30f;
                offsetZValue = 0f;
                minimapSizeSpeed = 0.2f;
                minimapSizeMin = 20f;
                minimapSizeMax = 100f;
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

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.RightBracket) && offsetYValue > minimapSizeMin)
        {
            minimapCamera.orthographicSize -= minimapSizeSpeed;
            if (minimapCamera.orthographicSize < minimapSizeMin)
            {
                minimapCamera.orthographicSize = minimapSizeMin;
            }
        }
        if (Input.GetKey(KeyCode.LeftBracket) && offsetYValue < minimapSizeMax)
        {
            minimapCamera.orthographicSize += minimapSizeSpeed;
            if (minimapCamera.orthographicSize > minimapSizeMax)
            {
                minimapCamera.orthographicSize = minimapSizeMax;
            }
        }
        activePlayerCharacter = tm.activePlayer.gameObject;
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