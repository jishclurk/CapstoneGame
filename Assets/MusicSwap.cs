using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSwap : MonoBehaviour {

    // Use this for initialization
    private AudioSource thisPlayer;
    private bool isTriggered;
    void Start () {
        thisPlayer = GetComponent<AudioSource>();
        isTriggered = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !other.isTrigger && !isTriggered)
        {
            isTriggered = true;
            thisPlayer.Play();
        }
    }
}
