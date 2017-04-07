using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TacticalPauseMenuTab : MonoBehaviour {

    public Sprite selectedSprite;
    public GameObject menu;

    private Image attachedImage;
    private Sprite defaultSprite;

    // Use this for initialization
    void Start () {
        attachedImage = GetComponent<Image>();
        defaultSprite = attachedImage.sprite;
    }
	
	// Update is called once per frame
	void Update () {
		if (menu.activeInHierarchy)
        {
            attachedImage.sprite = selectedSprite;
        }
        else
        {
            attachedImage.sprite = defaultSprite;
        }

	}
}
