using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour, IDropHandler {

    public ISpecial ability { get; set; }

    public int spot;

    public bool isSetSlot;

    private TacticalPause tp;

    private void Start()
    {
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

    public void OnDrop(PointerEventData eventData)
    {
        if (!item)
        {
           // Debug.Log(ability);
            Debug.Log(spot);
            ability = DragHandler.abilityBeingDraged;
            if (isSetSlot)
            {
                DragHandler.itemBeingDraged.transform.SetParent(transform);
                tp.updateAbilities(spot, ability); //needs to separately handle basic ability change?
            }else
            {
                spot = DragHandler.itemBeingDraged.transform.parent.GetComponent<SlotScript>().spot;
                DragHandler.itemBeingDraged.transform.SetParent(transform);
                tp.updateAbilities(spot, new EmptyAbility());
            }

            //ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasChanged());
        }
    }
}
