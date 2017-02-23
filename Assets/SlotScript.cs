using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour, IDropHandler {

    public IAbility ability { get; set; }

    public int spot;

    private TeamManager tm;

    private void Start()
    {
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();

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
            DragHandler.itemBeingDraged.transform.SetParent(transform);
            tm.combatPause.updateAbilities(spot, ability);
            //ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasChanged());
        }
    }
}
