using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CapstoneGame;

public class EnemyGoal : MonoBehaviour, IGoal {

    public GameManager manager;
    public AudioSource goalSound = new AudioSource();

    public void ScoreGoal()
    {
        goalSound.Play();
        manager.ScorePlayerGoal();
    }

}