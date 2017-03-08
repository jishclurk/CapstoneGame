using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetCamera : MonoBehaviour
{

    public GameObject activePlayerCharacter;
    private TeamManager tm;
    private Vector3 offset;
    private bool followPlayer;
    private bool useLookAt;
    private float radius;
    private float offsetYValue;
    private Vector3 fixedEulerAngles;
    private float cameraVerticalSpeed;
    private float cameraRotationSpeed;
    private float mouseScrollSpeed;
    private float yPositionMin;
    private float yPositionMax;
    private float xRotationMin;
    private float xRotationMax;

    //Simple script from Roll a Ball
    void Start()
    {
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
        activePlayerCharacter = tm.activePlayer.gameObject;
        float xOffset = -5f;
        float yOffset = 9f;
        float zOffset = -5f;
        offset = new Vector3(xOffset, yOffset, zOffset);
        transform.position = activePlayerCharacter.transform.position + offset;
        Debug.Log("offset:" + offset);
        followPlayer = false;
        Debug.Log("useLootAt is " + useLookAt);
        useLookAt = false;
        radius = Mathf.Sqrt(offset.x * offset.x + offset.z * offset.z);
        offsetYValue = offset.y;
        transform.eulerAngles = new Vector3(45f, 45f, 0f);
        fixedEulerAngles = transform.eulerAngles;
        cameraVerticalSpeed = 0.2f;
        cameraRotationSpeed = cameraVerticalSpeed * 10 / 6; // 10/6 is the ratio of the xRotation range over the yPosition range
        mouseScrollSpeed = 10f; 
        yPositionMin = yOffset - 3f;
        yPositionMax = yOffset + 3f;
        xRotationMin = 40f;
        xRotationMax = 50f;
}

    void LateUpdate()
    {
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
        if(Input.GetKeyDown(KeyCode.L))
        {
            if(useLookAt)
            {
                useLookAt = false;
                transform.eulerAngles = fixedEulerAngles;
                Debug.Log("useLootAt is " + useLookAt);
            }
            else
            {
                useLookAt = true; Debug.Log("useLootAt is " + useLookAt);
            }
        }
        if (Input.GetKey(KeyCode.Minus) && offsetYValue > yPositionMin)
        {
            offsetYValue -= cameraVerticalSpeed;
            if (offsetYValue < yPositionMin)
            {
                offsetYValue = yPositionMin;
            }
            offset = new Vector3(offset.x, offsetYValue, offset.z);
            if (useLookAt)
            {
                transform.LookAt(activePlayerCharacter.transform);
            }
            else
            {
                float xRotation = transform.eulerAngles.x - cameraRotationSpeed;
                if (xRotation < xRotationMin)
                {
                    xRotation = xRotationMin;
                }
                transform.eulerAngles = new Vector3(xRotation, transform.eulerAngles.y, transform.eulerAngles.z);
            }
        }
        if (Input.GetKey(KeyCode.Equals) && offsetYValue < yPositionMax)
        {
            offsetYValue += cameraVerticalSpeed;
            if (offsetYValue > yPositionMax)
            {
                offsetYValue = yPositionMax;
            }
            offset = new Vector3(offset.x, offsetYValue, offset.z);
            if (useLookAt)
            {
                transform.LookAt(activePlayerCharacter.transform);
            }
            else
            {
                float xRotation = transform.eulerAngles.x + cameraRotationSpeed;
                if (xRotation > xRotationMax)
                {
                    xRotation = xRotationMax;
                }
                transform.eulerAngles = new Vector3(xRotation, transform.eulerAngles.y, transform.eulerAngles.z);
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0 && offsetYValue >= yPositionMin && offsetYValue <= yPositionMax)
        {
            offsetYValue -= Input.GetAxis("Mouse ScrollWheel") * cameraVerticalSpeed * mouseScrollSpeed;
            if (offsetYValue < yPositionMin)
            {
                offsetYValue = yPositionMin;
            }
            else if (offsetYValue > yPositionMax)
            {
                offsetYValue = yPositionMax;
            }
            offset = new Vector3(offset.x, offsetYValue, offset.z);
            if (useLookAt)
            {
                transform.LookAt(activePlayerCharacter.transform);
            }
            else
            {
                float xRotation = transform.eulerAngles.x - Input.GetAxis("Mouse ScrollWheel") * cameraRotationSpeed * mouseScrollSpeed;
                if (xRotation < xRotationMin)
                {
                    xRotation = xRotationMin;
                }
                else if (xRotation > xRotationMax)
                {
                    xRotation = xRotationMax;
                }
                transform.eulerAngles = new Vector3(xRotation, transform.eulerAngles.y, transform.eulerAngles.z);
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
