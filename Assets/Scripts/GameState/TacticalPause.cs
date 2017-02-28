using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TacticalPause : MonoBehaviour {

    Canvas PauseScreen;
    public Canvas AbilitiesScreen;
    private Canvas myAbilitiesScreen;
    private bool inTacticalPause;
    private List<GameObject> unLockedAbitiesSlots;
    private List<GameObject> setAbilitesSlots;
    TeamManager tm;
    private GameObject attributesScreen;
    private int displayedPlayer;
    SimpleGameManager gm;
    private GameObject AvailablePoints;
    private GameObject Strength;
    private GameObject Intelligence;
    private GameObject Stamina;
    private Button confirmPoints;
    private Button strengthUp;
    private Button strengthDown;
    private Button StaminaUp;
    private Button StaminaDown;
    private Button IntelUp;
    private Button IntelDown;
    private int strengthFloor;
    private int staminaFloor;
    private int intelFloor;

    // Use this for initialization
    void Start() {
        PauseScreen = GetComponent<Canvas>();
        PauseScreen.enabled = false;
        myAbilitiesScreen = Instantiate(AbilitiesScreen) as Canvas;
        myAbilitiesScreen.enabled = false;
        inTacticalPause = false;
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
        AvailablePoints = attributesScreen.transform.Find("Available Points:").gameObject;
        Strength = attributesScreen.transform.Find("Strength").gameObject;
        Intelligence = attributesScreen.transform.Find("Intelligence").gameObject;
        Stamina = attributesScreen.transform.Find("Stamina").gameObject;
        confirmPoints = attributesScreen.transform.Find("Confirm").gameObject.GetComponent<Button>();
        strengthUp = Strength.transform.Find("+").gameObject.GetComponent<Button>();
        strengthDown = Strength.transform.Find("-").gameObject.GetComponent<Button>(); ;
        StaminaUp = Stamina.transform.Find("+").gameObject.GetComponent<Button>(); ;
        StaminaDown = Stamina.transform.Find("-").gameObject.GetComponent<Button>(); ;
        IntelUp = Intelligence.transform.Find("+").gameObject.GetComponent<Button>(); ;
        IntelDown = Intelligence.transform.Find("-").gameObject.GetComponent<Button>(); ;
        gm = SimpleGameManager.Instance;
    }

    // Update is called once per frame
    void Update () {
        if (inTacticalPause && !tm.IsTeamInCombat())
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
                    loadCurrentPlayerInfo(tm.getPlayerFromId(-1));
                }
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (displayedPlayer < 4)
                {
                    displayedPlayer++;
                }else
                {
                    displayedPlayer = 1;
                }
                loadCurrentPlayerInfo(tm.getPlayerFromId(displayedPlayer));
            }
        }
	}

    private void clearSlots()
    {
        foreach(GameObject slot in unLockedAbitiesSlots)
        {
            if (slot.transform.childCount > 0)
            {
                //slot.transform.GetChild(0).gameObject.SetActive(false);
                Destroy(slot.transform.GetChild(0).gameObject);
            }
        }

        foreach (GameObject slot in setAbilitesSlots)
        {
            if (slot.transform.childCount > 0)
            {
                //slot.transform.GetChild(0).gameObject.SetActive(false);
                Destroy(slot.transform.GetChild(0).gameObject);
            }
        }
    }

    //loads info from player with playerID, if id = -1, loads active player infor
    public void loadCurrentPlayerInfo(Player active)
    {
        clearSlots();
       // Debug.Log(playerID);
        //Player active = tm.getPlayerFromId(playerID);
        displayedPlayer = active.id;
        HashSet<Type> setAbilities = new HashSet<Type>();
        Debug.Log(active);
        Debug.Log(active.abilities);
        Debug.Log(active.abilities.abilityArray);
        for (int i = 0; i < 4; i++)
        {
            if (!active.abilities.abilityArray[i].GetType().Equals(typeof(EmptyAbility)))
            {
                Image image = GameObject.Instantiate(active.abilities.abilityArray[i].image) as Image;
                image.transform.SetParent(setAbilitesSlots[i].transform);
                //active.abilities.abilityArray[i].image.transform.SetParent(setAbilitesSlots[i].transform);
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
                Image image = GameObject.Instantiate(active.abilities.unlockedAbilities[i].image) as Image;
                image.transform.SetParent(unLockedAbitiesSlots[i].transform);
                //active.abilities.unlockedAbilities[i].image.transform.SetParent(unLockedAbitiesSlots[i].transform);
                Debug.Log(active.abilities.unlockedAbilities[i]);
                Debug.Log(i);
                Debug.Log(unLockedAbitiesSlots[i].GetComponent<SlotScript>().ability);
                Debug.Log(unLockedAbitiesSlots[i].GetComponent<SlotScript>().spot);
                unLockedAbitiesSlots[i].GetComponent<SlotScript>().ability = active.abilities.unlockedAbilities[i];
               // unLockedAbitiesSlots[i].GetComponent<SlotScript>().spot = i;
                Debug.Log(unLockedAbitiesSlots[i].GetComponent<SlotScript>().ability);
                Debug.Log(unLockedAbitiesSlots[i].GetComponent<SlotScript>().spot);
            }


        }

    }
      //  Debug.Log(setAbilitesSlots.Count);
       // Debug.Log(active.abilities.abilityArray.Length);

    
    public void updateAbilities(int spot, ISpecial ability)
    {
        Debug.Log(spot);
        Debug.Log(ability);
        Debug.Log(displayedPlayer);
        Debug.Log(tm.getPlayerFromId(displayedPlayer));
        tm.getPlayerFromId(displayedPlayer).abilities.SetNewAbility(ability, spot);
        ISpecial[] array = tm.getPlayerFromId(displayedPlayer).abilities.abilityArray;
        foreach(ISpecial x in array)
        {
            Debug.Log(x);
        }
    }

    public void Enable()
    {
        //combatPause = t
        PauseScreen.enabled = true;
        Time.timeScale = 0;
        inTacticalPause = true;
    }

    public void Disable()
    {
        myAbilitiesScreen.enabled = false;
        PauseScreen.enabled = false;
        Time.timeScale = 1;
        inTacticalPause = false;
    }

    public void ExitToMain()
    {
        gm.OnStateChange += LoadMainMenu;
        gm.SetGameState(GameState.MAIN_MENU);
    }

    private void LoadMainMenu()
    {
        Disable();
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

