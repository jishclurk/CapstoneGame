using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityPrefDropdownHelper : MonoBehaviour {

    public Dropdown abilityDropdown;
    public Dropdown targetDropdown;

    void Start () {
        abilityDropdown.onValueChanged.AddListener(delegate {
            abilityValueChangedHandler(abilityDropdown);
        });
    }

    void Destroy()
    {
        abilityDropdown.onValueChanged.RemoveAllListeners();
        targetDropdown.onValueChanged.RemoveAllListeners();
    }

    private void abilityValueChangedHandler(Dropdown target)
    {
        
    }

    private void targetValueChangedHandler(Dropdown target)
    {

    }
}
