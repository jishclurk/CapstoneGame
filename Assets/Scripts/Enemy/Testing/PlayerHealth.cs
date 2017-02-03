using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public float startingHealth = 100;
    public float currentHealth;
    
    public bool isDead { get { return currentHealth <= 0; } }
    private bool deathHandled = false;

	void Awake ()
    {
        currentHealth = startingHealth;
	}
	

	void Update ()
    {
		
	}

    public void TakeDamage (float amount)
    {
        currentHealth -= Mathf.Abs(amount);
        Debug.Log("Player Lost " + amount.ToString() + " Health");
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
