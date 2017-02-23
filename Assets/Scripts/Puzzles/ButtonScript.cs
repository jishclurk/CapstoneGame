using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {

    // Use this for initialization
   private AudioSource sound;
	private bool pressed;
	private bool disabled;

    void Start () {
        sound = GetComponent<AudioSource>();
		pressed = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider other)
    {
		if (!disabled) {
			if (!other.isTrigger && other.CompareTag ("Player")) {
				sound.Play ();
				Debug.Log ("Button Pressed");
				pressed = !pressed;
				//GameObject.FindGameObjectWithTag("Door").SetActive(false);
			}
		}
    }

	public bool Pressed(){
		return pressed;
	}

	public void Disable(){
		disabled = true;
	}
}
