using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public float maxHealth = 100;

    private float _currentHealth;
    public float currentHealth
    {
        get { return _currentHealth; }
        set
        {
            _currentHealth = value;
            if (_currentHealth < 0) _currentHealth = 0;
        }
    }

    public bool isDead { get { return currentHealth <= 0; } }
    public bool isDamaged { get { return currentHealth < maxHealth; } }
    private bool deathHandled = false;

	void Awake ()
    {
        currentHealth = maxHealth;
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
