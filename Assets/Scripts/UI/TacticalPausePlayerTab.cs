using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TacticalPausePlayerTab : MonoBehaviour {

    [Range(1, 4)]
    public int playerIndex = 1;
    public Sprite selectedSprite;

    private TeamManager tm;
    private Image attachedImage;
    private Sprite defaultSprite;
    private Button button;

	// Use this for initialization
	void Awake () {
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
        button = GetComponent<Button>();
        attachedImage = GetComponent<Image>();
        defaultSprite = attachedImage.sprite;
    }
	
	// Update is called once per frame
	void Update () {
        if (tm.getPlayerFromId(playerIndex).resources.isDead)
        {
            attachedImage.sprite = defaultSprite;
            button.interactable = false;
        }
        else if (tm.activePlayer.id == playerIndex)
        {
            attachedImage.sprite = selectedSprite;
            button.interactable = true;
        }
        else
        {
            attachedImage.sprite = defaultSprite;
            button.interactable = true;
        }
	}

    public void SwitchToPlayer()
    {
        tm.changeToActivePlayer(playerIndex);
    }
}
