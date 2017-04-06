using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour, IDropHandler {

    //ability in the slot
    public IAbility ability { get; set; }

    //id of ability assigned to the slot, if it is a tree slot, or the ability in a set slot
    public int abilityId;

    //spot in the ability array that the ability should be assigned to, if it is a bar slot
    public int spot;

    public bool isBarSlot;

    public bool isBasicBarSlot;

   // public bool isSpecialSlot;

    private TacticalPause tp;

    private void Start()
    {
        if (!isBarSlot)
        {
            ability = Utils.AbilityIDs[abilityId];
        }
        tp = GameObject.Find("TacticalPause").GetComponent<TacticalPause>();

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
            if (DragHandler.abilityBeingDraggedSlot.isBarSlot)
            {

                tp.updateSpecialAbilities(DragHandler.abilityBeingDraggedSlot.spot, new EmptyAbility());
            }

            Debug.Log("Good Drop");
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
                    transform.GetChild(0).SetParent(treeSlot, false);
                    DragHandler.itemBeingDragged.transform.SetParent(transform);
                    if (!isBasicBarSlot)
                    {
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
                    DragHandler.abilityBeingDraggedSlot.gameObject.transform.SetParent(treeSlot, false);

                }
                
            }
            ability = DragHandler.abilityBeingDraggedSlot.ability;

            if (isBarSlot)
            {
                abilityId = DragHandler.abilityBeingDraggedSlot.abilityId;
            }
            DragHandler.itemBeingDragged.transform.SetParent(transform);
            if (isBasicBarSlot)
            {
                tp.updateBasicAbility((IBasic)ability);
            }
            else
            {

                tp.updateSpecialAbilities(spot, (ISpecial)ability);
            }

        }
        else
        {
            //goes back to parent, in DragHandler
            Debug.Log("Bad Drop");
        }


        ////TODO: DRAG LOGIC
        //if (!item)
        //{
        //   // Debug.Log(ability);
        //    Debug.Log(spot);
        //    ability = DragHandler.abilityBeingDraggedSlot.ability;
        //    if (isBarSlot && (ability.GetAction() != AbilityHelper.Action.Basic) && isSpecialSlot) //isSpecial ability and it's a special slot? //S -> S
        //    {
        //        DragHandler.itemBeingDragged.transform.SetParent(transform);
        //        tp.updateSpecialAbilities(spot, (ISpecial) ability); //needs to separately handle basic ability change?
        //        //add swap functionality
        //    }
        //    else if (isBarSlot && (ability.GetAction() == AbilityHelper.Action.Basic) && !isSpecialSlot) //B -> B
        //    {

        //        //tp.updateBasicAbility((IBasic) ability);
        //    }
        //    else if(false){ //S -> anywhere but S

        //    } 
        //    else{ //B -> anywhere but B


        //        //reset original spot to empty if ability is dragged out of ability bar
        //        Debug.Log("bad drop!!");
        //        spot = DragHandler.itemBeingDragged.transform.parent.GetComponent<SlotScript>().spot;
        //        DragHandler.itemBeingDragged.transform.SetParent(transform);
        //        tp.updateSpecialAbilities(spot, new EmptyAbility());
        //    }

        //}
        
    }
}
