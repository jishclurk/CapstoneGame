﻿using System;
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
    private Transform Menus;
    private Transform ControlMenu;
    private GameObject attributesScreen;
    private GameObject CustomizeScreen;

    private List<GameObject> setAbilitesSlots;
    private List<GameObject> unLockedAbitiesSlots;
    private List<SlotScript> unLockedAbilitesSlotScripts;
    private GameObject basicAbilitySlot;

    private bool inTacticalPause;
    TeamManager tm;
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

    private Transform viewCharacterButton;

    // Use this for initialization
    void Start() {
        PauseScreen = GetComponent<Canvas>();
        PauseScreen.enabled = false;
        Menus = transform.Find("Menus");
        Menus.gameObject.SetActive(false);
        ControlMenu = transform.Find("ControlMenu");
        ControlMenu.gameObject.SetActive(false);
        viewCharacterButton = transform.FindChild("Character Menu");

        AbilitiesScreen = Menus.transform.Find("Abilities").gameObject;
        AbilitiesScreen.SetActive(false);
        CustomizeScreen = Menus.transform.Find("Customize").gameObject;
        CustomizeScreen.SetActive(false);
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
        gm = GameObject.Find("GameManager").GetComponent<SimpleGameManager>();
        
        unLockedAbitiesSlots = new List<GameObject>();
        setAbilitesSlots = new List<GameObject>();
        unLockedAbilitesSlotScripts = new List<SlotScript>();

        //get list of unlockedAbility slots
        Transform unlockedAbilities = AbilitiesScreen.transform.Find("AbilitiesPanel/UnlockedAbilities");
        for (int i = 0; i < unlockedAbilities.childCount; i++)
        {
            GameObject slot = unlockedAbilities.GetChild(i).gameObject;
            if (slot.tag.Equals("slot"))
            {
                unLockedAbitiesSlots.Add(slot);
                unLockedAbilitesSlotScripts.Add(slot.GetComponent<SlotScript>());
            }
        }

        //get list of set ability slots
        Transform setAbilities = AbilitiesScreen.transform.Find("AbilitiesPanel/SetAbilities ");
        for (int i = 0; i < 4; i++)
        {
            setAbilitesSlots.Add(setAbilities.GetChild(i).gameObject);
        }
        basicAbilitySlot = setAbilities.GetChild(4).gameObject;

    }
    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gm.gameState.Equals(GameState.COMABT_PAUSE))
            {
                gm.OnStateChange += Disable;
                gm.SetGameState(GameState.PLAY);
            }else
            {
                gm.OnStateChange += Enable;
                gm.SetGameState(GameState.COMABT_PAUSE);
            }
        }

        if (inTacticalPause && !tm.IsTeamInCombat())
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                Debug.Log("pressed c");
                toggleAbilityMenu();
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                displayedPlayer = tm.activePlayer; //note to Claudia, this is my "bandaid" fix for some nulls if you tab before pressing c, an excellent work around -claudia
                int displayedPlayerId = displayedPlayer.id;
                bool playerFound = false;
                while (!playerFound)
                {
                    if (displayedPlayerId < 4)
                    {
                        displayedPlayerId++;
                    }
                    else
                    {
                        displayedPlayerId = 1;
                    }
                    if (!tm.getPlayerFromId(displayedPlayerId).resources.isDead)
                    {
                        playerFound = true;
                    }
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
        if(basicAbilitySlot.transform.childCount > 0)
        {
            Destroy(basicAbilitySlot.transform.GetChild(0).gameObject);
        }
    }

    public void toggleAbilityMenu()
    {
        if (Menus.gameObject.activeInHierarchy)
        {
            Debug.Log("1");
            Menus.gameObject.SetActive(false);
            //PauseScreen.enabled = true;
            //AbilitiesScreen.SetActive(false);
        }
        else if(!tm.IsTeamInCombat())
        {
            Debug.Log("2");
            //PauseScreen.enabled = false;
            Menus.gameObject.SetActive(true);
            AbilitiesScreen.SetActive(true);
            CustomizeScreen.SetActive(false);
            displayedPlayer = tm.activePlayer;
            loadCurrentPlayerInfo(tm.activePlayer);
        }
    }

    public void toggleControlMenu()
    {
        if (ControlMenu.gameObject.activeInHierarchy)
        {
            ControlMenu.gameObject.SetActive(false);
            //PauseScreen.enabled = true;
            //AbilitiesScreen.SetActive(false);
        }
        else
        {
            //PauseScreen.enabled = false;
            ControlMenu.gameObject.SetActive(true);
        }
    }

    public GameObject FindSlotById(int id)
    {
        for(int i = 0; i<unLockedAbilitesSlotScripts.Count; i++)
        {
            if(unLockedAbilitesSlotScripts[i].abilityId == id)
            {
                return unLockedAbitiesSlots[i];
            }
        }
        Debug.Log("counldn't find the slot by id");
        return unLockedAbitiesSlots[0]; //added bc c# made me
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
                image.transform.SetParent(setAbilitesSlots[i].transform, false);
                setAbilities.Add(active.abilities.abilityArray[i].GetType());
            }
        }

        Image basicImage = GameObject.Instantiate(active.abilities.Basic.image) as Image;
        basicImage.transform.SetParent(basicAbilitySlot.transform, false);
        basicAbilitySlot.GetComponent<SlotScript>().ability = active.abilities.Basic;
        setAbilities.Add(active.abilities.Basic.GetType());

      //  Debug.Log(setAbilities);
      foreach(SlotScript slot in unLockedAbilitesSlotScripts)
        {
            slot.setDisplayedPlayer(active);
        }

        foreach (IAbility unlockedAbility in active.abilities.unlockedAbilities)
        {
            if (!setAbilities.Contains(unlockedAbility.GetType()))
            {
                GameObject abilityTreeSlot = FindSlotById(unlockedAbility.id);
                Image image = GameObject.Instantiate(unlockedAbility.image) as Image;
                image.transform.SetParent(abilityTreeSlot.transform, false);
            }

        }

        

        loadAttributesInfo();
    }

    //Loads the available abilities and set abilities of active
    public void loadPlayerInfoByID(int ID)
    {
        Player player = tm.getPlayerFromId(ID);
        displayedPlayer = player;
        loadCurrentPlayerInfo(player);
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

        if ((change < 0 && stamina > floorStamina) || (change > 0 && pointsLeft > 0))
        {
            stamina += change;
            pointsLeft -= change;
        }
        XPText.text = xpString + pointsLeft.ToString();
        StaminaText.text = staminaString + stamina.ToString();

    }

    //updates the displayed intell
    public void ChangeIntell(int change)
    {
        if ((change < 0 && intell > floorIntell) || (change>0 && pointsLeft>0))
        {
            intell += change;
            pointsLeft -= change;
        }
        XPText.text = xpString + pointsLeft.ToString();
        IntellText.text = intellString + intell.ToString();

    }

    //updates the displayed strength
    public void ChangeStrength(int change)
    {

        if ((change < 0 && strength > floorStrength) || (change > 0 && pointsLeft > 0))
        {
            strength += change;
            pointsLeft -= change;
        }
        XPText.text = xpString + pointsLeft.ToString();
        StrengthText.text = strengthString + strength.ToString();
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
    public void updateSpecialAbilities(int spot, ISpecial ability)
    {
        displayedPlayer.abilities.SetNewAbility(ability.id, spot);
        ISpecial[] array = displayedPlayer.abilities.abilityArray;
        displayedPlayer.attributes.ResetPassiveBonus();
        foreach(ISpecial x in array)
        {
            x.updatePassiveBonuses(displayedPlayer.attributes);
        }
    }
    //puts basic ability into the displayed players Basic
    public void updateBasicAbility(IBasic ability)
    {
        displayedPlayer.abilities.Basic = ability;

    }

    //Stops game, turns on pause screen
    public void Enable()
    {
        PauseScreen.enabled = true;
        Time.timeScale = 0;
        inTacticalPause = true;
        ControlMenu.gameObject.SetActive(false); //idk I added these here and everything now works -nick
        Menus.gameObject.SetActive(false);
        if (tm.IsTeamInCombat())
        {
            viewCharacterButton.GetComponent<Button>().interactable = false;
        } else
        {
            viewCharacterButton.GetComponent<Button>().interactable = true;
        }
    }

    public void Disable()
    {
        CustomizeScreen.SetActive(false);
        AbilitiesScreen.SetActive(false);
        PauseScreen.enabled = false;
        Time.timeScale = 1;
        inTacticalPause = false;
        Menus.gameObject.SetActive(false);
        ControlMenu.gameObject.SetActive(false);

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


