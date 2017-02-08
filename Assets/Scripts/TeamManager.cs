using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour {

    private GameObject[] playerList;
    private List<Strategy> strategyList;
    public GameObject activePlayer;
    public Strategy activeStrat;
    public bool isTeamInCombat;

    private OffsetCamera cameraScript;


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
        //set current player to AI control
        activeStrat.setAsCoopAI();

        //find next available player's strategy and set as Player control
        int nextPlayer = (strategyList.IndexOf(activeStrat) + 1) % strategyList.Count;
        activeStrat = strategyList[nextPlayer];
        activeStrat.setAsPlayer();

        //update activePlayer and camera
        activePlayer = activeStrat.gameObject;
        cameraScript.followPlayer = activePlayer;
    }



    public void SpawnTeamMember()
    {

        
    }
}
