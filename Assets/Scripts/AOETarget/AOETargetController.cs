using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOETargetController : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.CompareTag("Floor"))
            {
                transform.position = hit.point;
            }
        }
    }
}