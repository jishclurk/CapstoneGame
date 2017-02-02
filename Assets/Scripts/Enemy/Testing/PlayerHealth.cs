using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public int startingHealth = 100;
    public int currentHealth;
    
    public bool isDead { get { return currentHealth <= 0; } }
    private bool deathHandled = false;

	void Awake ()
    {
        currentHealth = startingHealth;
	}
	

	void Update ()
    {
		
	}

    public void TakeDamage (int amount)
    {
        currentHealth -= amount;
        if (isDead && !deathHandled)
        {
            Death();
        }
    }

    private void Death()
    {
        deathHandled = true;
        Debug.Log("Player died!");
        // Animation stuff goes here
    }
}
