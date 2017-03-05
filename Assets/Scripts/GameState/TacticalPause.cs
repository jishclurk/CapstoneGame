using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Supports ability to pause game, change players abilities and attribute values if not in combat
public class TacticalPause : MonoBehaviour {

    Canvas PauseScreen;
    public GameObject AbilitiesScreen;

    private bool inTacticalPause;
    private List<GameObject> unLockedAbitiesSlots;
    private List<GameObject> setAbilitesSlots;
    TeamManager tm;
    private GameObject attributesScreen;
    private Player displayedPlayer;
    SimpleGameManager gm;

    //attribute variables
    private int floorStrength;
    private int strength;
    private int floorStamina;
    private int stamina;
    private int floorIntell;
    private int intell;
    private int pointsLeft;

    private Text XPText;
    private Text StrengthText;
    private Text IntellText;
    private Text StaminaText;

    private string xpString;
    private string strengthString;
    private string intellString;
    private string staminaString;

    // Use this for initialization
    void Start() {
        PauseScreen = GetComponent<Canvas>();
        PauseScreen.enabled = false;
        AbilitiesScreen = transform.GetChild(1).gameObject;
        AbilitiesScreen.SetActive(false);
        attributesScreen = AbilitiesScreen.transform.Find("Attributes").gameObject;

        XPText = attributesScreen.transform.GetChild(0).GetComponent<Text>();
        StrengthText = attributesScreen.transform.GetChild(1).GetComponent<Text>();
        IntellText = attributesScreen.transform.GetChild(2).GetComponent<Text>();
        StaminaText = attributesScreen.transform.GetChild(3).GetComponent<Text>();

        xpString = XPText.text;
        strengthString = StrengthText.text;
        intellString = IntellText.text;
        staminaString = StaminaText.text;

        inTacticalPause = false;
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
        gm = SimpleGameManager.Instance;

        unLockedAbitiesSlots = new List<GameObject>();
        setAbilitesSlots = new List<GameObject>();

        //get list of unlockedAbility slots
        Transform unlockedAbilities = AbilitiesScreen.transform.Find("AbilitiesPanel/UnlockedAbilities");
        for (int i = 0; i < unlockedAbilities.childCount; i++)
        {
            unLockedAbitiesSlots.Add(unlockedAbilities.GetChild(i).gameObject);
        }

        //get list of set ability slots
        Transform setAbilities = AbilitiesScreen.transform.Find("AbilitiesPanel/SetAbilities ");
        for (int i = 0; i < setAbilities.childCount; i++)
        {
            setAbilitesSlots.Add(setAbilities.GetChild(i).gameObject);
        }

    }

    // Update is called once per frame
    void Update () {
        if (inTacticalPause && !tm.IsTeamInCombat())
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                toggleAbilityMenu();
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                int displayedPlayerId = displayedPlayer.id;
                if (displayedPlayerId < 4)
                {
                    displayedPlayerId++;
                }else
                {
                    displayedPlayerId = 1;
                }
                displayedPlayer = tm.getPlayerFromId(displayedPlayerId);
                loadCurrentPlayerInfo(displayedPlayer);
            }
        }
	}

    // clears all slots
    private void clearSlots()
    {
        foreach(GameObject slot in unLockedAbitiesSlots)
        {
            if (slot.transform.childCount > 0)
            {
                Destroy(slot.transform.GetChild(0).gameObject);
            }
        }

        foreach (GameObject slot in setAbilitesSlots)
        {
            if (slot.transform.childCount > 0)
            {
                Destroy(slot.transform.GetChild(0).gameObject);
            }
        }
    }

    public void toggleAbilityMenu()
    {
        if (AbilitiesScreen.activeInHierarchy)
        {
            PauseScreen.enabled = true;
            AbilitiesScreen.SetActive(false);
        }
        else
        {
            //PauseScreen.enabled = false;
            AbilitiesScreen.SetActive(true);
            displayedPlayer = tm.activePlayer;
            loadCurrentPlayerInfo(tm.activePlayer);
        }
    }

    //Loads the available abilities and set abilities of active
    public void loadCurrentPlayerInfo(Player active)
    {
        clearSlots();
        HashSet<Type> setAbilities = new HashSet<Type>();
        for (int i = 0; i < 4; i++)
        {
            if (!active.abilities.abilityArray[i].GetType().Equals(typeof(EmptyAbility)))
            {
                Image image = GameObject.Instantiate(active.abilities.abilityArray[i].image) as Image;
                image.transform.SetParent(setAbilitesSlots[i].transform);
                setAbilities.Add(active.abilities.abilityArray[i].GetType());
            }
        }
        Debug.Log(setAbilities);

        for (int i = 0; i < active.abilities.unlockedSpecialAbilities.Count; i++)
        {
            Debug.Log(i);
            if (!setAbilities.Contains(active.abilities.unlockedSpecialAbilities[i].GetType()))
            {
                Image image = GameObject.Instantiate(active.abilities.unlockedSpecialAbilities[i].image) as Image;
                image.transform.SetParent(unLockedAbitiesSlots[i].transform);
                unLockedAbitiesSlots[i].GetComponent<SlotScript>().ability = active.abilities.unlockedSpecialAbilities[i];
            }
        }

        loadAttributesInfo();
    }

    //Loads attribute values of displayed player onto the screeen
    private void loadAttributesInfo()
    {
        floorIntell = displayedPlayer.attributes.Intelligence;
        floorStamina = displayedPlayer.attributes.Stamina;
        floorStrength = displayedPlayer.attributes.Strength;
        pointsLeft = displayedPlayer.attributes.StatPoints;
        strength = floorStrength;
        intell = floorIntell;
        stamina = floorStamina;

        XPText.text = xpString + pointsLeft.ToString();
        StrengthText.text = strengthString + floorStrength.ToString();
        IntellText.text = intellString + floorIntell.ToString();
        StaminaText.text = staminaString + floorStamina.ToString();

    }

    //updates the displayed stamina
    public void ChangeStamina(int change)
    {
        Debug.Log("button pressed");
        if (pointsLeft > 0)
        {
            if (change > 0 || stamina > floorStamina)
            {
                stamina += change;
            pointsLeft -= change;
            }

            XPText.text = xpString + pointsLeft.ToString();
            StaminaText.text = staminaString + stamina.ToString();
        }

    }

    //updates the displayed intell
    public void ChangeIntell(int change)
    {
        Debug.Log("button pressed");

        if (pointsLeft > 0)
        {
            if (change > 0 || intell > floorIntell)
            {
                intell += change;
            pointsLeft -= change;
            }
            XPText.text = xpString + pointsLeft.ToString();
            IntellText.text = intellString + intell.ToString();
        }

    }

    //updates the displayed strength
    public void ChangeStrength(int change)
    {

        if (pointsLeft > 0)
        {
            if (change > 0 || strength > floorStrength)
            {
                strength += change;
                pointsLeft -= change;

            }

            XPText.text = xpString + pointsLeft.ToString();
            StrengthText.text = strengthString + strength.ToString();
        }

    }

    //Updates displayed players attributes with displayed attribute values
    public void ConfirmAttributeValues()
    {
        displayedPlayer.attributes.StatPoints = pointsLeft;
        displayedPlayer.attributes.Strength = strength;
        displayedPlayer.attributes.Intelligence = intell;
        displayedPlayer.attributes.Stamina = stamina;
        displayedPlayer.abilities.UpdateUnlockedAbilities(displayedPlayer.attributes);
        loadCurrentPlayerInfo(displayedPlayer);
    }
    //puts ability into the displayed players ability array at spot
    public void updateAbilities(int spot, ISpecial ability)
    {
        displayedPlayer.abilities.SetNewAbility(ability, spot);
        ISpecial[] array = displayedPlayer.abilities.abilityArray;

        //prints players ability array for debugging 
        foreach(ISpecial x in array)
        {
            Debug.Log(x);
        }
    }

    //Stops game, turns of pause screen
    public void Enable()
    {
        PauseScreen.enabled = true;
        Time.timeScale = 0;
        inTacticalPause = true;
    }

    public void Disable()
    {
        AbilitiesScreen.SetActive(false);
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

    
}


