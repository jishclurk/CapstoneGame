using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTrigger : MonoBehaviour {

    public Canvas CheckPointSceen;

    public void Start()
    {
        CheckPointSceen = Instantiate(CheckPointSceen) as Canvas;
        CheckPointSceen = CheckPointSceen.GetComponent<Canvas>();
        CheckPointSceen.enabled = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Time.timeScale = 0;
            CheckPointSceen.enabled = true;
        }
    }

}
