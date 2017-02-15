using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour {

    private GameObject[] gObjList;
    private List<Player> playerList;
    public Player activePlayer;
    //public Strategy activeStrat;

    private OffsetCamera cameraScript;
    //private CharacterAttributes[] characterAttributesArray;

	public PlayerResources playerResources;

    public List<GameObject> visibleEnemies;


    // Use this for initialization
    void Start () {
        gObjList = GameObject.FindGameObjectsWithTag("Player");

        playerList = new List<Player>();
        //assign active player to user controller player
        for (int i = 0; i < gObjList.Length; i++)
        {
            Player player = gObjList[i].GetComponent<Player>();

            playerList.Add(player);
            Debug.Log(player);
            Debug.Log(player.strategy);
            if (player.strategy.isplayerControlled)
            {
                activePlayer = player;
            }
        }

        cameraScript = Camera.main.GetComponent<OffsetCamera>(); //subject to change
        visibleEnemies = new List<GameObject>();
        GameManager.manager.SetTeamManager(this);

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            cycleActivePlayer();
        }
    }

    //eventually will take a parameter to change certain player
    public void cycleActivePlayer()
    {
        //find next available player's strategy and set as Player control
        int nextPlayer = (playerList.IndexOf(activePlayer) + 1) % playerList.Count;
        PlayerResources nextResources = playerList[nextPlayer].resources;
        if (!nextResources.isDead)
        {
            //set current player to AI control
            if (!activePlayer.resources.isDead)
            {
                activePlayer.strategy.setAsCoopAI();
                //TODO: change over enemyclicked to aiPlayer.targetedEnemy and updated WatchedPlayers.
            }

            //set next player as player Control
            activePlayer = playerList[nextPlayer];
            activePlayer.strategy.setAsPlayer();

            //update activePlayer and camera
            playerResources = nextResources;
            cameraScript.followPlayer = activePlayer.gameObject;
        } else
        {
            Debug.Log("Cannot switch to a dead player!");
        }

    }


    public bool isTeamInCombat()
    {
        return visibleEnemies.Count > 0;
    } 


    public void AwardExperience(int experiencePoints)
    {
        for (int i = 0; i < gObjList.Length; i++)
        {
            playerList[i].attributes.Experience += experiencePoints;
        }
    }

    public SerializedPlayer[] currentState()
    {
        SerializedPlayer[] players = new SerializedPlayer[gObjList.Length];
       /* for (int i = 0; i < gObjList.Length; i++)
        {
            SerializedPlayer player = new SerializedPlayer();
            player.level = characterAttributesArray[i].Level;
            player.experience = characterAttributesArray[i].Experience;
           // player.experienceNeededForNextLevel = characterAttributesArray[i].;
            player.strength = characterAttributesArray[i].Strength;
            player.intelligence = characterAttributesArray[i].Intelligence;
            player.stamina = characterAttributesArray[i].Stamina;
            player.isInControl = playerList[i].isplayerControlled;
            players[i] = player;
        }
        */
        return players;
    }

    public void LoadSavedState(SerializedPlayer[] state)
    {


        /*
        for (int i = 0; i<gObjList.Length; i++)
        {
            characterAttributesArray[i].Level = state[i].level;
            characterAttributesArray[i].Experience = state[i].experience;
            characterAttributesArray[i].Strength = state[i].strength;
            characterAttributesArray[i].Intelligence = state[i].intelligence;
            characterAttributesArray[i].Stamina = state[i].stamina;

            if (state[i].isInControl)
            {
                activeStrat = playerList[i];
                activePlayer = playerList[i].gameObject;
            }

        }
        */
    }
}
