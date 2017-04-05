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
    private Transform Menus;
    private GameObject attributesScreen;

    private List<GameObject> setAbilitesSlots;
    private List<GameObject> unLockedAbitiesSlots;

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

    // Use this for initialization
    void Start() {
        PauseScreen = GetComponent<Canvas>();
        PauseScreen.enabled = false;
        Menus = transform.Find("Menus");
        Menus.gameObject.SetActive(false);

        AbilitiesScreen = Menus.transform.Find("Abilities").gameObject;
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
        gm = GameObject.Find("GameManager").GetComponent<SimpleGameManager>();
        
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gm.gameState.Equals(GameState.COMABT_PAUSE))
            {
                gm.OnStateChange += Disable;
                //PauseScreen.enabled = false;
                //Menus.gameObject.SetActive(false);
                //Time.timeScale = 1;
                gm.SetGameState(GameState.PLAY);
                //inTacticalPause = false;  
            }else
            {

                gm.OnStateChange += Enable;
                gm.SetGameState(GameState.COMABT_PAUSE);
            }
        }

    //         public void StartComabtPause()
    //{
    //    if (gm.gameState.Equals(GameState.COMABT_PAUSE))
    //    {
    //        gm.OnStateChange += tacticalPause.Disable;
    //        gm.SetGameState(GameState.PLAY);
    //    }
    //    else
    //    {
    //        gm.OnStateChange += tacticalPause.Enable;
    //        gm.SetGameState(GameState.COMABT_PAUSE);
    //    }
    //}
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
        if (Menus.gameObject.activeInHierarchy)
        {
            Debug.Log("1");
            Menus.gameObject.SetActive(false);
            //PauseScreen.enabled = true;
            //AbilitiesScreen.SetActive(false);
        }
        else
        {
            Debug.Log("2");
            //PauseScreen.enabled = false;
            Menus.gameObject.SetActive(true);
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
                image.transform.SetParent(setAbilitesSlots[i].transform, false);
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
                image.transform.SetParent(unLockedAbitiesSlots[i].transform, false);
                unLockedAbitiesSlots[i].GetComponent<SlotScript>().ability = active.abilities.unlockedSpecialAbilities[i];
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
    public void updateAbilities(int spot, ISpecial ability)
    {
        displayedPlayer.abilities.SetNewAbility(ability, spot);
        ISpecial[] array = displayedPlayer.abilities.abilityArray;

        //prints players ability array for debugging 
        //Nick: Added Passive bonus allocation here for each ability
        displayedPlayer.attributes.ResetPassiveBonus();
        foreach(ISpecial x in array)
        {
            x.updatePassiveBonuses(displayedPlayer.attributes);
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


