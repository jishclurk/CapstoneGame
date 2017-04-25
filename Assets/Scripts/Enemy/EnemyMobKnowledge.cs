using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyMobKnowledge : MonoBehaviour {

    public enum MobStrategy { Even, Random }

    public MobStrategy strategy;

    public bool AllEnemiesDead { get; private set; }

    private Dictionary<GameObject, int> playerVisibilityMap;
    private Dictionary<GameObject, int> playerAggroMap;

    private int enemiesInMob;
    private int numEnemiesDead;

	// Use this for initialization
	void Awake () {
        playerVisibilityMap = new Dictionary<GameObject, int>();
        playerAggroMap = new Dictionary<GameObject, int>();

        AllEnemiesDead = false;
        numEnemiesDead = 0;
        enemiesInMob = transform.childCount;
    }

	public List<GameObject> GetVisiblePlayers()
    {
        return new List<GameObject>(this.playerVisibilityMap.Keys);
    }

    public void AddVisiblePlayer(GameObject player)
    {
        if (playerVisibilityMap.ContainsKey(player)) {
            playerVisibilityMap[player] += 1;
        }
        else
        {
            playerVisibilityMap.Add(player, 1);
        }

        if (!playerAggroMap.ContainsKey(player))
            playerAggroMap.Add(player, 0);
    }

    public void RemoveVisiblePlayer(GameObject player)
    {
        if (playerVisibilityMap.ContainsKey(player) && playerVisibilityMap[player] > 0)
        {
            playerVisibilityMap[player] -= 1;

            if (playerVisibilityMap[player] <= 0)
            {
                playerVisibilityMap.Remove(player);
                playerAggroMap.Remove(player);
            }
        }
    }

    public GameObject GetNewTarget()
    {
        if (strategy == MobStrategy.Random)
        {
            GameObject newTarget = playerVisibilityMap.ElementAt(Random.Range(0, playerVisibilityMap.Count)).Key;
            if (!playerAggroMap.ContainsKey(newTarget))
                playerAggroMap.Add(newTarget, 1);
            else
                playerAggroMap[newTarget] += 1;
            return newTarget;
        }
        else // Even strategy (spread out). Default.
        {
            int minAggro = playerAggroMap.Values.Min();
            GameObject newTarget = null;
            foreach (GameObject player in playerVisibilityMap.Keys)
            {
                if (playerAggroMap.ContainsKey(player) && playerAggroMap[player] == minAggro)
                {
                    playerAggroMap[player] += 1;
                    return player;
                }
                else if (!playerAggroMap.ContainsKey(player))
                {
                    playerAggroMap.Add(player, 1);
                    return player;
                }
                else
                    newTarget = player;
            }

            playerAggroMap[newTarget] += 1;
            return newTarget;
        }
    }

    public void RemoveTargettedPlayer(GameObject player)
    {
        if (playerAggroMap.ContainsKey(player) && playerAggroMap[player] > 0)
        {
            playerAggroMap[player] -= 1;
        }
    }

    public void RemoveKnowledgeOfPlayer(GameObject player)
    {
        if (playerVisibilityMap.ContainsKey(player))
            playerVisibilityMap.Remove(player);
        if (playerAggroMap.ContainsKey(player))
            playerAggroMap.Remove(player);
    }

    public void SignalEnemyDead()
    {
        numEnemiesDead += 1;

        if (numEnemiesDead >= enemiesInMob)
            AllEnemiesDead = true;
    }
}
