using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPause : MonoBehaviour {

    Canvas PauseScreen;
    public Canvas AbilitiesScreen;
    private bool inCombatPause;
    private List<GameObject> unLockedAbitiesSlots;
    private List<GameObject> setAbilitesSlots;
   // private bool setAbilitiesMode;
    TeamManager tm;
	// Use this for initialization
	void Start () {
        PauseScreen = GetComponent<Canvas>();
        PauseScreen.enabled = false;
        AbilitiesScreen = Instantiate(AbilitiesScreen) as Canvas;
        AbilitiesScreen.enabled = false;
        inCombatPause = false;
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
        //unLockedAbitiesSlots = 
        //for(int i = 0; i< AbilitiesScreen.transform.GetChild(2).transform.GetChild(0).childCount; i++)
        //{
        //    unLockedAbitiesSlots[i] = AbilitiesScreen.transform.GetChild(2).transform.GetChild(0).GetChild(i).gameObject;
        //}

        //for (int i = 0; i < AbilitiesScreen.transform.GetChild(2).transform.GetChild(1).childCount; i++)
        //{
        //    setAbilitesSlots[i] = AbilitiesScreen.transform.GetChild(2).transform.GetChild(1).GetChild(i).gameObject;
        //}

        //Debug.Log(unLockedAbitiesSlots);
        //Debug.Log(setAbilitesSlots);
        //setAbilitiesMode = false

    }

    // Update is called once per frame
    void Update () {
        if (inCombatPause && !tm.IsTeamInCombat())
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (AbilitiesScreen.enabled)
                {
                    PauseScreen.enabled = true;
                    AbilitiesScreen.enabled = false;
                }
                else
                {
                    PauseScreen.enabled = false;
                    AbilitiesScreen.enabled = true;
                }
            }
        }
	}

    private void loadUnlockedAbilities()
    {
        List<IAbility> activeAbilities = tm.activePlayer.abilities.unlockedAbilities;
        for (int i= 0; i< activeAbilities.Count; i++)
        {

        }
        //GameObject UnlockedAbilitesBar = AbilitiesScreen.
    }

    public void Enable()
    {
        //combatPause = t
        PauseScreen.enabled = true;
        Time.timeScale = 0;
        inCombatPause = true;
    }

    public void Disable()
    {
        AbilitiesScreen.enabled = false;
        PauseScreen.enabled = false;
        Time.timeScale = 1;
        inCombatPause = false;
    }
}
