using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatPause : MonoBehaviour {

    Canvas PauseScreen;
    public Canvas AbilitiesScreen;
    private bool inCombatPause;
    private List<GameObject> unLockedAbitiesSlots;
    private List<GameObject> setAbilitesSlots;
    TeamManager tm;
    private GameObject attributesScreen;

    // Use this for initialization
    void Start() {
        PauseScreen = GetComponent<Canvas>();
        PauseScreen.enabled = false;
        AbilitiesScreen = Instantiate(AbilitiesScreen) as Canvas;
        AbilitiesScreen.enabled = false;
        inCombatPause = false;
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();

        unLockedAbitiesSlots = new List<GameObject>();
        setAbilitesSlots = new List<GameObject>();

        for (int i = 0; i < AbilitiesScreen.transform.GetChild(2).transform.GetChild(0).childCount; i++)
        {
            unLockedAbitiesSlots.Add(AbilitiesScreen.transform.GetChild(2).transform.GetChild(0).GetChild(i).gameObject);
        }

        for (int i = 0; i < AbilitiesScreen.transform.GetChild(2).transform.GetChild(1).childCount; i++)
        {
            setAbilitesSlots.Add(AbilitiesScreen.transform.GetChild(2).transform.GetChild(1).GetChild(i).gameObject);
        }

        attributesScreen = AbilitiesScreen.transform.GetChild(8).gameObject;
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
                    loadCurrentPlayerInfo(-1);
                }
            }
        }
	}

    //loads info from player with playerID, if id = -1, loads active player infor
    public void loadCurrentPlayerInfo(int playerID)
    {

        Player active = tm.getPlayerFromId(playerID);

        for (int i= 0; i< active.abilities.unlockedAbilities.Count; i++)
        {
            unLockedAbitiesSlots[i].GetComponent<Image>().sprite = active.abilities.unlockedAbilities[i].image.sprite;
        }


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
