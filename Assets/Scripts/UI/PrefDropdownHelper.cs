using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefDropdownHelper : MonoBehaviour {

    public Dropdown abilityDropdown;
    public Dropdown targetDropdown;
    public Text abilityText;
    public Text targetText;

    private TeamManager tm;

    void Awake () {
        abilityDropdown.onValueChanged.AddListener(delegate {
            abilityValueChangedHandler(abilityDropdown);
        });

        targetDropdown.onValueChanged.AddListener(delegate {
            targetValueChangedHandler(targetDropdown);
        });

        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
    }

    void Update()
    {
        switch (tm.activePlayer.strategy.aiScript.abilityChoose)
        {
            case Strategy.AbilityPref.Agressive:
                abilityDropdown.value = 0;
                abilityText.text = "Quick";
                break;
            case Strategy.AbilityPref.Offensive:
                abilityDropdown.value = 1;
                abilityText.text = "Offensive";
                break;
            case Strategy.AbilityPref.Defensive:
                abilityDropdown.value = 2;
                abilityText.text = "Defensive";
                break;
            case Strategy.AbilityPref.Balanced:
                abilityDropdown.value = 3;
                abilityText.text = "Balanced";
                break;
            case Strategy.AbilityPref.None:
                abilityDropdown.value = 4;
                abilityText.text = "None";
                break;
        }

        switch (tm.activePlayer.strategy.aiScript.targetChoose)
        {
            case Strategy.TargetPref.Closest:
                targetDropdown.value = 0;
                targetText.text = "Closest";
                break;
            case Strategy.TargetPref.Lowest:
                targetDropdown.value = 1;
                targetText.text = "Lowest Health";
                break;
            case Strategy.TargetPref.Active:
                targetDropdown.value = 2;
                targetText.text = "Active Player Target";
                break;
        }
    }

    void Destroy()
    {
        abilityDropdown.onValueChanged.RemoveAllListeners();
        targetDropdown.onValueChanged.RemoveAllListeners();
    }

    private void abilityValueChangedHandler(Dropdown target)
    {
        switch (target.value)
        {
            case 0:
                tm.activePlayer.strategy.SetCoopAbilityPref(Strategy.AbilityPref.Agressive);
                break;
            case 1:
                tm.activePlayer.strategy.SetCoopAbilityPref(Strategy.AbilityPref.Offensive);
                break;
            case 2:
                tm.activePlayer.strategy.SetCoopAbilityPref(Strategy.AbilityPref.Defensive);
                break;
            case 3:
                tm.activePlayer.strategy.SetCoopAbilityPref(Strategy.AbilityPref.Balanced);
                break;
            case 4:
                tm.activePlayer.strategy.SetCoopAbilityPref(Strategy.AbilityPref.None);
                break;
        }
    }

    private void targetValueChangedHandler(Dropdown target)
    {
        switch (target.value)
        {
            case 0:
                tm.activePlayer.strategy.SetCoopTargetPref(Strategy.TargetPref.Closest);
                break;
            case 1:
                tm.activePlayer.strategy.SetCoopTargetPref(Strategy.TargetPref.Lowest);
                break;
            case 2:
                tm.activePlayer.strategy.SetCoopTargetPref(Strategy.TargetPref.Active);
                break;
        }
    }
}
