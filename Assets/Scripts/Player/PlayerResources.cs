using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour {

    public float maxHealth = 100;

    private float _currentHealth;
    public float currentHealth
    {
        get { return _currentHealth; }
        set
        {
            _currentHealth = value;
            if (_currentHealth < 0) _currentHealth = 0;
            if (_currentHealth > maxHealth) _currentHealth = maxHealth;
        }
    }

    public float maxEnergy = 100;
    private float _currentEnergy;
    public float currentEnergy
    {
        get { return _currentEnergy; }
        set
        {
            _currentEnergy = value;
            if (_currentEnergy< 0) _currentHealth = 0;
            if (_currentEnergy > maxEnergy) _currentEnergy = currentEnergy;
        }
    }
    public float energyRegenRateInSeconds = 2.0f;
    public float energyRegenAmt = 1.0f;

    public bool isDead { get { return currentHealth <= 0; } }
    public bool isDamaged { get { return currentHealth < maxHealth; } }
    private bool deathHandled = false;

	void Awake ()
    {
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
        InvokeRepeating("RegenerateEnergy", 1.0f, energyRegenRateInSeconds);
	}
	

	void Update ()
    {
        
		
	}

    public void TakeDamage (float amount)
    {
        currentHealth -= Mathf.Abs(amount);
        Debug.Log("Player Lost " + amount.ToString() + " Health");
        Debug.Log("Player Health:  " + currentHealth);
        if (isDead && !deathHandled)
        {
            Death();
        }
    }

    public void Heal(float amount)
    {
        if (!isDead)
        {
            currentHealth += Mathf.Abs(amount);
            Debug.Log("Player Health:  " + currentHealth);
        }
    }

    public void UseEnergy(float amount)
    {
        currentEnergy -= Mathf.Abs(amount);
        Debug.Log("Player Lost " + amount.ToString() + " Energy");
        Debug.Log("Player Energy:  " + currentEnergy);
    }

    public void RegenerateEnergy()
    {
        currentEnergy += energyRegenAmt;
    }

    private void Death()
    {
        deathHandled = true;
        Debug.Log("Player died!");
        // Animation stuff goes here
    }
}
