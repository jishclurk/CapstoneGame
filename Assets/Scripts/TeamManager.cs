using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour {

    private GameObject[] playerList;
    private List<Strategy> strategyList;
    public GameObject activePlayer;


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
                activePlayer = strat.gameObject;
            }
        }
    }

    void Start()
    {

    }


    public void SpawnTeamMember()
    {


    }
}
