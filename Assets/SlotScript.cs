using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour, IDropHandler {

    public IAbility ability { get; set; }

    public int abilityId;

    public int spot;

    public bool isBarSlot;

    public bool isSpecialSlot;

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

    public void OnDrop(PointerEventData eventData)
    {
        //TODO: DRAG LOGIC
        if (!item)
        {
           // Debug.Log(ability);
            Debug.Log(spot);
            ability = DragHandler.abilityBeingDraged;
            if (isBarSlot && (ability.GetAction() != AbilityHelper.Action.Basic) && isSpecialSlot) //isSpecial ability and it's a special slot? //S -> S
            {
                DragHandler.itemBeingDraged.transform.SetParent(transform);
                tp.updateSpecialAbilities(spot, (ISpecial) ability); //needs to separately handle basic ability change?
                //add swap functionality
            }
            else if (isBarSlot && (ability.GetAction() == AbilityHelper.Action.Basic) && !isSpecialSlot) //B -> B
            {

                //tp.updateBasicAbility((IBasic) ability);
            }
            else if(false){ //S -> anywhere but S

            } 
            else{ //B -> anywhere but B


                //reset original spot to empty if ability is dragged out of ability bar
                Debug.Log("bad drop!!");
                spot = DragHandler.itemBeingDraged.transform.parent.GetComponent<SlotScript>().spot;
                DragHandler.itemBeingDraged.transform.SetParent(transform);
                tp.updateSpecialAbilities(spot, new EmptyAbility());
            }

            //ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasChanged());
        }
    }
}
