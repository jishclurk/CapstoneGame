using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadMenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject slots = transform.FindChild("slots").gameObject;
        SaveButton[] buttons = new SaveButton[5];
        for(int i = 0; i<buttons.Length; i++)
        {
            GameObject button = slots.transform.GetChild(i).gameObject;
            bool somethingSavedInSlot = SaveLoad.setSaveButton(button.GetComponent<SaveButton>());
            if (!somethingSavedInSlot)
            {
                button.GetComponent<Button>().interactable = false;
            }
        }
	}
	
}
