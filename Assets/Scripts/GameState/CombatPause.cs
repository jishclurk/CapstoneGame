using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CombatPause : MonoBehaviour {

    Canvas PauseScreen;
    public Canvas AbilitiesScreen;
    private Canvas myAbilitiesScreen;
    private bool inCombatPause;
    private List<GameObject> unLockedAbitiesSlots;
    private List<GameObject> setAbilitesSlots;
    TeamManager tm;
    private GameObject attributesScreen;
    private int displayedPlayer;
    SimpleGameManager gm;

    // Use this for initialization
    void Start() {
        PauseScreen = GetComponent<Canvas>();
        PauseScreen.enabled = false;
        myAbilitiesScreen = Instantiate(AbilitiesScreen) as Canvas;
        myAbilitiesScreen.enabled = false;
        inCombatPause = false;
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();

        unLockedAbitiesSlots = new List<GameObject>();
        setAbilitesSlots = new List<GameObject>();

        Transform unlockedAbilities = myAbilitiesScreen.transform.Find("AbilitiesPanel/UnlockedAbilities");
        for (int i = 0; i < unlockedAbilities.childCount; i++)
        {
            unLockedAbitiesSlots.Add(unlockedAbilities.GetChild(i).gameObject);
        }

        Transform setAbilities = myAbilitiesScreen.transform.Find("AbilitiesPanel/SetAbilities ");
        for (int i = 0; i < setAbilities.childCount; i++)
        {
            setAbilitesSlots.Add(setAbilities.GetChild(i).gameObject);
        }

        attributesScreen = myAbilitiesScreen.transform.Find("Attributes").gameObject;
        gm = SimpleGameManager.Instance;
    }

    // Update is called once per frame
    void Update () {
        /*if (inCombatPause && !tm.IsTeamInCombat())
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (myAbilitiesScreen.enabled)
                {
                    PauseScreen.enabled = true;
                    myAbilitiesScreen.enabled = false;
                }
                else
                {
                    PauseScreen.enabled = false;
                    myAbilitiesScreen.enabled = true;
                    loadCurrentPlayerInfo(-1);
                }
            }
        }*/
	}

    //loads info from player with playerID, if id = -1, loads active player infor
    public void loadCurrentPlayerInfo(int playerID)
    {

        Player active = tm.getPlayerFromId(playerID);
        displayedPlayer = active.id;
        HashSet<Type> setAbilities = new HashSet<Type>();
        Debug.Log(active);
        Debug.Log(active.abilities);
        Debug.Log(active.abilities.abilityArray);
        for (int i = 0; i < 4; i++)
        {
            if (!active.abilities.abilityArray[i].GetType().Equals(typeof(EmptyAbility)))
            {
                active.abilities.abilityArray[i].image.transform.SetParent(setAbilitesSlots[i].transform);
                setAbilities.Add(active.abilities.abilityArray[i].GetType());
            }
        }
        Debug.Log(setAbilities);

        //Debug.Log(unLockedAbitiesSlots.Count);
        //Debug.Log(active.abilities.unlockedAbilities.Count);
        for (int i = 0; i < active.abilities.unlockedAbilities.Count; i++)
        {
            Debug.Log(i);
            if (!setAbilities.Contains(active.abilities.unlockedAbilities[i].GetType()))
            {
                active.abilities.unlockedAbilities[i].image.transform.SetParent(unLockedAbitiesSlots[i].transform);
                unLockedAbitiesSlots[i].GetComponent<SlotScript>().ability = active.abilities.unlockedAbilities[i];
                unLockedAbitiesSlots[i].GetComponent<SlotScript>().spot = i;
            }


        }
    }
      //  Debug.Log(setAbilitesSlots.Count);
       // Debug.Log(active.abilities.abilityArray.Length);

    
    public void updateAbilities(int spot, IAbility ability)
    {
        tm.getPlayerFromId(displayedPlayer).abilities.abilityArray[spot] = ability;
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
        myAbilitiesScreen.enabled = false;
        PauseScreen.enabled = false;
        Time.timeScale = 1;
        inCombatPause = false;
    }

    public void ExitToMain()
    {
        gm.OnStateChange += LoadMainMenu;
        gm.SetGameState(GameState.MAIN_MENU);
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //public void HasChanged()
    //{
    //    Debug.Log("asdfasdf");
    //    throw new NotImplementedException();
    //}
}

//namespace UnityEngine.EventSystems
//{
//    public interface IHasChanged : IEventSystemHandler
//    {
//        void HasChanged();
//    }
//}

