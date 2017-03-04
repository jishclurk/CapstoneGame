using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour {
    public TacticalPause tacticalPause;

    public GameObject[] prefabList = new GameObject[4];
    public List<Player> playerList;
    public Player activePlayer;
    //public Strategy activeStrat;

    private OffsetCamera cameraScript;
    //private CharacterAttributes[] characterAttributesArray;

	public PlayerResources playerResources;

    public HashSet<GameObject> visibleEnemies;
    private SimpleGameManager gm;


    // Use this for initialization
    void Awake () {
        for(int i = 0; i<prefabList.Length; i++)
        {
            prefabList[i] = Instantiate(prefabList[i]);
        }

        tacticalPause = GameObject.Find("TacticalPause").GetComponent<TacticalPause>();
        gm = SimpleGameManager.Instance;

       // prefabList = GameObject.FindGameObjectsWithTag("Player");

        playerList = new List<Player>();
        //assign active player to user controller player
        for (int i = 0; i < prefabList.Length; i++)
        {
            Player player = prefabList[i].GetComponent<Player>();
            //player.id = i+1;
            Debug.Log("ID: " + player.id);

            playerList.Add(player);

            //Debug.Log(player);
            //Debug.Log(player.strategy);
            if (player.strategy.isplayerControlled)
            {
                activePlayer = player;
            }
        }

        playerList.Sort((x, y) => x.id - y.id);

        cameraScript = Camera.main.GetComponent<OffsetCamera>(); //subject to change
        cameraScript.activePlayerCharacter = activePlayer.gameObject;
        visibleEnemies = new HashSet<GameObject>();
        playerResources = activePlayer.GetComponent<PlayerResources>();

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartComabtPause();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            cycleActivePlayer();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            changeToActivePlayer(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            changeToActivePlayer(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            changeToActivePlayer(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            changeToActivePlayer(4);
        }

    }

    public Player getPlayerFromId(int id)
    {
        foreach (Player player in playerList)
        {
            if (player.id == id)
            {
                return player;
            }
        }
        return activePlayer; 
    }

    public void setPlayers(CheckPoint checkpoint)
    {
        playerList[0].gameObject.transform.position = checkpoint.player1;
        playerList[1].gameObject.transform.position = checkpoint.player2;
        playerList[2].gameObject.transform.position = checkpoint.player3;
        playerList[3].gameObject.transform.position = checkpoint.player4;

    }

    public void changeToActivePlayer(int id)
    {
        if (!playerList[id-1].resources.isDead)
        {//set current player to AI control
            if (!activePlayer.resources.isDead)
            {
                activePlayer.strategy.setAsCoopAI();
            }
            //set next player as player Control
            activePlayer = playerList[id-1];
            activePlayer.strategy.setAsPlayer();

            //update activePlayer and camera
            playerResources = playerList[id-1].resources;
            cameraScript.activePlayerCharacter = activePlayer.gameObject;
        }
        else
        {
            Debug.Log("Cannot switch! Player " + id + " is dead.");
        }

    }

    //find a way to not do this shit
    public void loadPlayerOnTactiaclPause(int id)
    {
        tacticalPause.loadCurrentPlayerInfo(getPlayerFromId(id));
    }

    //eventually will take a parameter to change certain player
    public void cycleActivePlayer()
    {
        //find next available player's strategy and set as Player control
        int nextPlayer = (playerList.IndexOf(activePlayer) + 1) % playerList.Count;
        PlayerResources nextResources = playerList[nextPlayer].resources;
        while (nextResources.isDead && nextPlayer != playerList.IndexOf(activePlayer))
        {
            nextPlayer = (nextPlayer + 1) % playerList.Count;
            nextResources = playerList[nextPlayer].resources;
        }
        if (!nextResources.isDead)
        {
            //set current player to AI control
            if (!activePlayer.resources.isDead)
            {
                activePlayer.strategy.setAsCoopAI();

            }

            //set next player as player Control
            activePlayer = playerList[nextPlayer];
            activePlayer.strategy.setAsPlayer();

            //update activePlayer and camera
            playerResources = nextResources;
            cameraScript.activePlayerCharacter = activePlayer.gameObject;
        } else
        {
            Debug.Log("Cannot switch! All players are dead.");
        }
    

    }


    public bool IsTeamInCombat()
    {
        return visibleEnemies.Count > 0;
    }

    public void StartComabtPause()
    {
        if (gm.gameState.Equals(GameState.COMABT_PAUSE))
        {
            gm.OnStateChange += tacticalPause.Disable;
            gm.SetGameState(GameState.PLAY);
        }
        else
        {
            gm.OnStateChange += tacticalPause.Enable;
            gm.SetGameState(GameState.COMABT_PAUSE);
        }
    }

    public void RemoveDeadEnemy(GameObject enemy)
    {
        visibleEnemies.Remove(enemy);
        foreach (Player p in playerList)
        {
            p.visibleEnemies.Remove(enemy);
            p.watchedEnemies.Remove(enemy);
        }
    }

    public void RemoveEnemyIfNotTeamVisible(GameObject enemy)
    {
        bool toRemove = true;
        foreach(Player p in playerList)
        {
            if (p.visibleEnemies.Contains(enemy))
            {
                toRemove = false;
            }
        }
        if (toRemove)
        {
            visibleEnemies.Remove(enemy);
        }
    }


    public void AwardExperience(int experiencePoints)
    {
        for (int i = 0; i < prefabList.Length; i++)
        {
            playerList[i].attributes.Experience += experiencePoints;
        }
    }


    public SerializedPlayer[] currentState()
    {
        SerializedPlayer[] players = new SerializedPlayer[playerList.Count];
        for (int i = 0; i < playerList.Count; i++)
        {
            CharacterAttributes current = playerList[i].attributes;
            SerializedPlayer player = new SerializedPlayer();
            player.level =current.Level;
            player.experience = current.Experience;
            player.strength = current.Strength;
            player.intelligence = current.Intelligence;
            player.stamina = current.Stamina;
            player.isInControl = playerList[i].Equals(activePlayer);
            player.id = playerList[i].id;
            player.statPoints = current.StatPoints;
            players[i] = player;
        }
        
        return players;
    }

    public void LoadSavedState(SerializedPlayer[] state)
    {
        for (int i = 0; i<playerList.Count; i++)
        {
            SerializedPlayer playerState = state[i];
            Player currentPlayer = getPlayerFromId(playerState.id);
            CharacterAttributes current = currentPlayer.attributes;
            current.Level = playerState.level;
            current.Experience = playerState.experience;
            current.StatPoints = playerState.statPoints;
            current.Strength = playerState.strength;
            current.Intelligence = playerState.intelligence;
            current.Stamina = playerState.statPoints;

            if (playerState.isInControl)
            {
                activePlayer = currentPlayer;
                currentPlayer.strategy.isplayerControlled = true;
                playerResources = activePlayer.GetComponent<PlayerResources>();
                cameraScript = Camera.main.GetComponent<OffsetCamera>(); 
                cameraScript.activePlayerCharacter = activePlayer.gameObject;
            }

        }
        
    }
}
