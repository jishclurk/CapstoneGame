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
    }

    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Minus) && offsetYValue > 6f)
        {
            offsetYValue -= 0.2f;
            offset = new Vector3(offset.x, offsetYValue, offset.z);
        }
        if (Input.GetKey(KeyCode.Equals) && offsetYValue < 12f)
        {
            offsetYValue += 0.2f;
            offset = new Vector3(offset.x, offsetYValue, offset.z);
        }
        if(offsetYValue > 12f)
        {
            offsetYValue = 12f;
        }
        else if(offsetYValue < 6f)
        {
            offsetYValue = 6f;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (followPlayer)
            {
                followPlayer = false;
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
        transform.LookAt(activePlayerCharacter.transform);
    }

}
