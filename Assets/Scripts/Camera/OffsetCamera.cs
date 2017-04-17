using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetCamera : MonoBehaviour
{

    public GameObject activePlayerCharacter;
    private Vector3 offset;
    private bool followPlayer;
    private float radius;
    private int numberOfAngles;
    private float[] angles;
    private int anglesIndex;
    private float offsetYValue;
    private Vector3 fixedEulerAngles;
    private float cameraVerticalSpeed;
    private float cameraRotationSpeed;
    private float mouseScrollSpeed;
    private float yPositionMin;
    private float yPositionMax;
    private float xRotationMin;
    private float xRotationMax;
    private float[] xzRotationArray;
    private int xzRotationArrayIndex;

    void Start()
    {
        offsetYValue = 9f;
        followPlayer = false;
        radius = 7f;
        numberOfAngles = 8;
        angles = new float[numberOfAngles];
        for(int i = 0; i < numberOfAngles; i++)
        {
            angles[i] = i * 2 * Mathf.PI / numberOfAngles;
        }
        anglesIndex = 0;
        offset = new Vector3(radius * Mathf.Sin(angles[anglesIndex]), offsetYValue, radius * Mathf.Cos(angles[anglesIndex]));
        fixedEulerAngles = transform.eulerAngles;
        cameraVerticalSpeed = 0.2f;
        cameraRotationSpeed = cameraVerticalSpeed * 10 / 6; // (10/6) is the ratio of the xRotation range over the yPosition range
        mouseScrollSpeed = 10f; 
        yPositionMin = offsetYValue - 3f;
        yPositionMax = offsetYValue + 3f;
        xRotationMin = 40f;
        xRotationMax = 50f;
    }

    public void setCamera(GameObject player)
    {
        activePlayerCharacter = player;
        transform.position = activePlayerCharacter.transform.position + offset;
        SmoothLookAt();
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z))
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
        if (Input.GetKeyDown(KeyCode.S))
        {
            anglesIndex--;
            if (anglesIndex < 0)
            {
                anglesIndex = 7;
            }
            offset = new Vector3(radius * Mathf.Sin(angles[anglesIndex]), offset.y, radius * Mathf.Cos(angles[anglesIndex]));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            anglesIndex++;
            if(anglesIndex >= numberOfAngles)
            {
                anglesIndex = 0;
            }
            offset = new Vector3(radius * Mathf.Sin(angles[anglesIndex]), offset.y, radius * Mathf.Cos(angles[anglesIndex]));
        }
        if (Input.GetKey(KeyCode.Equals) && offsetYValue > yPositionMin)
        {
            offsetYValue -= cameraVerticalSpeed;
            if (offsetYValue < yPositionMin)
            {
                offsetYValue = yPositionMin;
            }
            offset = new Vector3(offset.x, offsetYValue, offset.z);
        }
        if (Input.GetKey(KeyCode.Minus) && offsetYValue < yPositionMax)
        {
            offsetYValue += cameraVerticalSpeed;
            if (offsetYValue > yPositionMax)
            {
                offsetYValue = yPositionMax;
            }
            offset = new Vector3(offset.x, offsetYValue, offset.z);
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
        }
        if (followPlayer)
        {
            float angle = Mathf.Deg2Rad * activePlayerCharacter.transform.eulerAngles.y - Mathf.PI;
            Vector3 newPosition = new Vector3(activePlayerCharacter.transform.position.x + (radius * Mathf.Sin(angle)), offsetYValue, activePlayerCharacter.transform.position.z + (radius * Mathf.Cos(angle)));
            transform.position = Vector3.Lerp(transform.position, newPosition, 0.2f);
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
        SmoothLookAt();
    }

    private void SmoothLookAt()
    {
        Quaternion targetRotation = Quaternion.LookRotation(activePlayerCharacter.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.2f);
    }
}
