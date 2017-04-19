using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    //ability in the slot
    public IAbility ability { get; set; }

    //id of ability assigned to the slot, if it is a tree slot, or the ability in a set slot
    public int abilityId;

    //spot in the ability array that the ability should be assigned to, if it is a bar slot
    public int spot;

    public bool isBarSlot;

    public bool isBasicBarSlot;
    public GameObject hoverBubble;
    private Text hoverText;
    private Text abilityName;
    private Text AbilityType;
    private Player displayedPlayer;
    private TacticalPause tp;

    private void Start()
    {
        if (!isBarSlot)
        {
            ability = Utils.AbilityIDs[abilityId];
        }
        tp = GameObject.Find("TacticalPause").GetComponent<TacticalPause>();
        if (!hoverBubble.Equals(null))
        {
            hoverText = hoverBubble.transform.FindChild("description").GetComponent<Text>();
            abilityName = hoverBubble.transform.FindChild("name").GetComponent<Text>();
            AbilityType = hoverBubble.transform.FindChild("type").GetComponent<Text>();

            abilityName.text = ability.name;
            AbilityType.text = ability.useType;
            hoverBubble.SetActive(false);
        }
    }

    public void setDisplayedPlayer(Player player)
    {
        displayedPlayer = player;
    }

    public GameObject item
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    private bool isValidDrop()
    {
        bool isValid = true;
        if (DragHandler.itemBeingDragged.transform.parent.GetComponent<SlotScript>().isBasicBarSlot || //basic slot can't be left open
            (DragHandler.abilityBeingDraggedSlot.ability.GetAction() == AbilityHelper.Action.Basic && !isBasicBarSlot ) || //basic is trying to move into special slot
            (DragHandler.abilityBeingDraggedSlot.ability.GetAction()!= AbilityHelper.Action.Basic && isBasicBarSlot)) //special is trying to move into basic slot
        {
            isValid = false;
        }
            return isValid;
    }

    //handles drop into slots
    public void OnDrop(PointerEventData eventData)
    {
        if (isValidDrop())
        {
            Debug.Log("Good Drop");
            if (DragHandler.abilityBeingDraggedSlot.isBarSlot)
            {
                Debug.Log("leaving bar slot");
                tp.updateSpecialAbilities(DragHandler.abilityBeingDraggedSlot.spot, new EmptyAbility());
            }

            if (item) 
            {
                Debug.Log("Something in the slot");
                if (isBarSlot)
                {
                    Debug.Log("Swap");
                    if(ability == null)
                    {
                       // ability = item.get
                    }
                    Transform treeSlot = tp.FindSlotById(ability.id).transform;
                    ability = DragHandler.abilityBeingDraggedSlot.ability;
                    transform.GetChild(0).SetParent(treeSlot, false);
                    DragHandler.itemBeingDragged.transform.SetParent(transform);
                    if (!isBasicBarSlot)
                    {
                        Debug.Log("update non basic");
                         tp.updateSpecialAbilities(spot, (ISpecial)ability);
                    }
                    else
                    {
                        Debug.Log(ability);
                        ability = DragHandler.abilityBeingDraggedSlot.ability;
                        Debug.Log(ability);
                        tp.updateBasicAbility((IBasic)ability);
                    }
                }
                else
                {
                    Transform treeSlot = tp.FindSlotById(DragHandler.abilityBeingDraggedSlot.abilityId).transform;
                    DragHandler.itemBeingDragged.transform.SetParent(treeSlot, false);

                }

            }else
            {
                Debug.Log("nothing in the spot");
                if (isBarSlot)
                {
                    ability = DragHandler.abilityBeingDraggedSlot.ability;
                    if (isBasicBarSlot)
                    {
                        Debug.Log("Shouln't ever come here??");
                        tp.updateBasicAbility((IBasic)ability);

                    }
                    else
                    {
                        tp.updateSpecialAbilities(spot, (ISpecial)ability);
                    }
                    abilityId = DragHandler.abilityBeingDraggedSlot.abilityId;
                    DragHandler.itemBeingDragged.transform.SetParent(transform, false);
                }
                else
                {

                    Transform treeSlot = tp.FindSlotById(DragHandler.abilityBeingDraggedSlot.abilityId).transform;
                    Debug.Log("moving to slot of ability with id" + DragHandler.abilityBeingDraggedSlot.abilityId);
                    DragHandler.itemBeingDragged.transform.SetParent(treeSlot, false);
                    if (DragHandler.abilityBeingDraggedSlot.isBarSlot)
                    {
                        Debug.Log("moving from bar");
                        tp.updateSpecialAbilities(DragHandler.abilityBeingDraggedSlot.spot, new EmptyAbility());
                    }
                }



            }
           

        }
        else
        {
            //goes back to parent, in DragHandler
            Debug.Log("Bad Drop");
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!hoverBubble.Equals(null))
        {
            hoverBubble.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!hoverBubble.Equals(null))
        {
            hoverText.text = ability.GetHoverDescription(displayedPlayer);
            hoverBubble.SetActive(true);
           // hoverBubble.transform.position = new Vector3(Input.mousePosition.x + bubbleWidth / 2, Input.mousePosition.y + bubbleHeight / 2, hoverBubble.transform.position.z);
        }
    }
}
