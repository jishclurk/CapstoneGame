using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public static GameObject itemBeingDragged;
    Vector3 startPosition;
  //  public static IAbility abilityBeingDragged;
    Transform startParent;
    public static SlotScript abilityBeingDraggedSlot;
    private TacticalPause tp;

    void Start()
    {
        tp = GameObject.Find("TacticalPause").GetComponent<TacticalPause>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        startParent = transform.parent;
        itemBeingDragged = gameObject;
        startPosition = transform.position;
        abilityBeingDraggedSlot = transform.parent.gameObject.GetComponent<SlotScript>();
        GetComponent<CanvasGroup>().blocksRaycasts = false;

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    //handles drop into non-slot
    public void OnEndDrag(PointerEventData eventData)
    {

        itemBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if (transform.parent == startParent)
        {
            if (!abilityBeingDraggedSlot.isBasicBarSlot) //if the ability being draged is not from the basic slot
            {
                //Debug.Log("non basic dragged to non slot");
                //Debug.Log(abilityBeingDraggedSlot.abilityId);
                Transform treeSpot = tp.FindSlotById(abilityBeingDraggedSlot.abilityId).transform;
                transform.SetParent(treeSpot, false);
                transform.position = treeSpot.position;
                if (abilityBeingDraggedSlot.isBarSlot) //reset ability bar
                {
                    tp.updateSpecialAbilities(abilityBeingDraggedSlot.spot, new EmptyAbility());
                }
            }
            else
            {
                //Debug.Log("basic being draggeed somewhere");
                transform.position = startPosition;
            }

        }
       // transform.parent.gameObject.GetComponent<SlotScript>().ability = ability;
        
    }

}
