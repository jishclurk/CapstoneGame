using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour {

    private GameObject[] playerList;
    private List<Strategy> strategyList;
    public GameObject activePlayer;
    public Strategy activeStrat;
    public bool teamInCombat;

    private OffsetCamera cameraScript;

	public PlayerResources playerResources;


    // Use this for initialization
    void Awake () {
        playerList = GameObject.FindGameObjectsWithTag("Player");
        strategyList = new List<Strategy>();
        //assign active player to user controller player
        for (int i = 0; i < playerList.Length; i++)
        {
            Strategy strat = playerList[i].GetComponent<Strategy>();
            strategyList.Add(strat);
            if (strat.isplayerControlled)
            {
                activeStrat = strat;
                activePlayer = strat.gameObject;
            }
        }

        cameraScript = Camera.main.GetComponent<OffsetCamera>(); //subject to change
    }

    void Start()
    {

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
        int nextPlayer = (strategyList.IndexOf(activeStrat) + 1) % strategyList.Count;

        if (!strategyList[nextPlayer].GetComponent<PlayerResources>().isDead)
        {
            //set current player to AI control
            if (!activeStrat.GetComponent<PlayerResources>().isDead)
            {
                activeStrat.setAsCoopAI();
            }

            //set next player as player Control
            activeStrat = strategyList[nextPlayer];
            activeStrat.setAsPlayer();

            //update activePlayer and camera
            activePlayer = activeStrat.gameObject;
            cameraScript.followPlayer = activePlayer;
        } else
        {
            Debug.Log("Cannot switch to a dead player!");
        }

    }


    public bool isTeamInCombat()
    {
        if (teamInCombat)
        {
            return teamInCombat;
        }
        bool playerCombat = false;
        bool aiCombat = true;
        foreach (Strategy strat in strategyList)
        {
            if (strat.isplayerControlled)
            {
                playerCombat = strat.gameObject.GetComponent<PlayerController>().enemyClicked;
            }
            else
            {
                aiCombat = strat.gameObject.GetComponent<CoopAiController>().currentState == strat.gameObject.GetComponent<CoopAiController>().attackState;
            }
        }
        return playerCombat || aiCombat;
    }


    public void SpawnTeamMember()
    {

        
    }
}
