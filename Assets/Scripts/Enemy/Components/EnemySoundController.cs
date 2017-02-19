using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundController : MonoBehaviour {

    public List<AudioClip> aggroSounds;
    public List<AudioClip> attackSounds;
    public List<AudioClip> deathSounds;

    private AudioSource source;

    public void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayAggroSound()
    {
        source.clip = aggroSounds[(int)(Random.Range(0, aggroSounds.Count))];
        source.Play();
    }

    public void PlayAttackSound()
    {
        source.clip = attackSounds[(int)(Random.Range(0, attackSounds.Count))];
        source.Play();
    }

    public void PlayDeathSound()
    {
        source.clip = deathSounds[(int)(Random.Range(0, deathSounds.Count))];
        source.Play();
    }
}
