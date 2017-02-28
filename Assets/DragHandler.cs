﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public static GameObject itemBeingDraged;
    Vector3 startPosition;
    public static ISpecial abilityBeingDraged;
    Transform startParent;

    public void OnBeginDrag(PointerEventData eventData)
    {
       // ISpecial ability = transform.parent.gameObject.GetComponent<SlotScript>().ability;
        startParent = transform.parent;
        itemBeingDraged = gameObject;
        startPosition = transform.position;
        abilityBeingDraged = transform.parent.gameObject.GetComponent<SlotScript>().ability;
        GetComponent<CanvasGroup>().blocksRaycasts = false;

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeingDraged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if (transform.parent == startParent)
        {
            transform.position = startPosition;
        }
       // transform.parent.gameObject.GetComponent<SlotScript>().ability = ability;
        
    }

}
