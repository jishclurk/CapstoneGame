
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
{

    public Canvas CheckPointSceen;

    public bool checkpointReached;

    public void Start()
    {
        CheckPointSceen = Instantiate(CheckPointSceen) as Canvas;
        CheckPointSceen = CheckPointSceen.GetComponent<Canvas>();
        CheckPointSceen.enabled = false;
        checkpointReached = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            checkpointReached = true;
            //            Time.timeScale = 0;
            //            CheckPointSceen.enabled = true;
        }
    }

    public void openCheckPointScreen()
    {

        // Time.timeScale = 0;
        CheckPointSceen.enabled = true;
    }
}