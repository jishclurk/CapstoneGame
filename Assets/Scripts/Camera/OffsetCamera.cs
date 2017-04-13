using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetCamera : MonoBehaviour
{

    public GameObject activePlayerCharacter;
   // private TeamManager tm;
    private Vector3 offset;
    private bool followPlayer;
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
    private float[] xzRotationArray;
    private int xzRotationArrayIndex;

    //Simple script from Roll a Ball
    void Start()
    {
       // tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
        //activePlayerCharacter = tm.activePlayer.gameObject;
        float offsetXValue = 5f;
        offsetYValue = 9f;
        float offsetZValue = 5f;
        xzRotationArray = new float[8];
        xzRotationArray[0] = offsetXValue;
        xzRotationArray[1] = offsetZValue;
        xzRotationArray[2] = -offsetXValue;
        xzRotationArray[3] = offsetZValue;
        xzRotationArray[4] = -offsetXValue;
        xzRotationArray[5] = -offsetZValue;
        xzRotationArray[6] = offsetXValue;
        xzRotationArray[7] = -offsetZValue;
        xzRotationArrayIndex = 4;
        offset = new Vector3(xzRotationArray[xzRotationArrayIndex], offsetYValue, xzRotationArray[xzRotationArrayIndex + 1]);
       // SmoothLookAt();
        Debug.Log("offset:" + offset);
        followPlayer = false;
        radius = Mathf.Sqrt(offset.x * offset.x + offset.z * offset.z);
        fixedEulerAngles = transform.eulerAngles;
        cameraVerticalSpeed = 0.2f;
        cameraRotationSpeed = cameraVerticalSpeed * 10 / 6; // 10/6 is the ratio of the xRotation range over the yPosition range
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

    void FixedUpdate()
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
        if (Input.GetKeyDown(KeyCode.A))
        {
            xzRotationArrayIndex += 2;
            if(xzRotationArrayIndex > 7)
            {
                xzRotationArrayIndex = 0;
            }
            offset = new Vector3(xzRotationArray[xzRotationArrayIndex], offset.y, xzRotationArray[xzRotationArrayIndex + 1]);
            SmoothLookAt();
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
