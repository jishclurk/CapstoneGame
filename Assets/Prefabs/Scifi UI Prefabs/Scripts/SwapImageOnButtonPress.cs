using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapImageOnButtonPress : MonoBehaviour {

    public Sprite imageToSwap;
    public KeyCode keyToSwap;
    private Image defaultImage;
    private Sprite defaultSprite;

	void Awake () {
        defaultImage = GetComponent<Image>();
        defaultSprite = defaultImage.sprite;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(keyToSwap))
            defaultImage.sprite = imageToSwap;
        else if (Input.GetKeyUp(keyToSwap))
            defaultImage.sprite = defaultSprite;
    }
}
