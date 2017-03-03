using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetCamera : MonoBehaviour
{

    public GameObject activePlayerCharacter;
    private TeamManager tm;
    private Vector3 offset;
    private bool followPlayer;
    private float radius;
    private float offsetYValue;
    private Vector3 fixedEulerAngles;


    //Simple script from Roll a Ball
    void Start()
    {
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
        activePlayerCharacter = tm.activePlayer.gameObject;
        offset = transform.position - activePlayerCharacter.transform.position;
        Debug.Log("offset:" + offset);
        followPlayer = false;
        radius = Mathf.Sqrt(offset.x * offset.x + offset.z * offset.z);
        offsetYValue = offset.y;
        fixedEulerAngles = transform.eulerAngles;
    }

    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Minus) && offsetYValue > 6f)
        {
            offsetYValue -= 0.2f;
            if (offsetYValue > 12f)
            {
                offsetYValue = 12f;
            }
            else if (offsetYValue < 6f)
            {
                offsetYValue = 6f;
            }
            offset = new Vector3(offset.x, offsetYValue, offset.z);
        }
        if (Input.GetKey(KeyCode.Equals) && offsetYValue < 12f)
        {
            if (offsetYValue > 12f)
            {
                offsetYValue = 12f;
            }
            else if (offsetYValue < 6f)
            {
                offsetYValue = 6f;
            }
            offsetYValue += 0.2f;
            offset = new Vector3(offset.x, offsetYValue, offset.z);
        }
        
        if (Input.GetAxis("Mouse ScrollWheel") != 0 && offsetYValue >= 6f && offsetYValue <= 12f)
        {
            offsetYValue -= Input.GetAxis("Mouse ScrollWheel");
            float xRotation = transform.eulerAngles.x - Input.GetAxis("Mouse ScrollWheel");
            if(xRotation < 40f)
            {
                xRotation = 40f;
            }
            else if(xRotation > 50f)
            {
                xRotation = 50f;
            }
            if (offsetYValue > 12f)
            {
                offsetYValue = 12f;
            }
            else if (offsetYValue < 6f)
            {
                offsetYValue = 6f;
            }
            transform.eulerAngles = new Vector3(xRotation, transform.eulerAngles.y, transform.eulerAngles.z);
            offset = new Vector3(offset.x, offsetYValue, offset.z);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (followPlayer)
            {
                followPlayer = false;
                transform.eulerAngles = fixedEulerAngles;
            }
            else
            {
                followPlayer = true;
            }
        }
        if (followPlayer)
        {
            float angle = Mathf.Deg2Rad * activePlayerCharacter.transform.eulerAngles.y - Mathf.PI;
            Vector3 newPosition = new Vector3(activePlayerCharacter.transform.position.x + (radius * Mathf.Sin(angle)), offsetYValue, activePlayerCharacter.transform.position.z + (radius * Mathf.Cos(angle)));
            transform.position = Vector3.Lerp(transform.position, newPosition, 0.2f);
            transform.LookAt(activePlayerCharacter.transform);
        }
        else
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

}
