using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour {

    public static float maxHealth = 100;

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

    public static float maxEnergy = 100;
    private float _currentEnergy;
    public float currentEnergy
    {
        get { return _currentEnergy; }
        set
        {
            _currentEnergy = value;
            if (_currentEnergy < 0) _currentEnergy = 0;
            if (_currentEnergy > maxEnergy) _currentEnergy = maxEnergy;
        }
    }
    public float energyRegenRateInSeconds = 0.5f;
    public float energyRegenAmt = 0.5f;

    private PlayerAnimationController animController;
    private TeamManager tm;
    private CharacterAttributes attributes;
    public bool isDead { get { return currentHealth <= 0; } }
    public bool isDamaged { get { return currentHealth < maxHealth; } }
    private bool deathHandled = false;

    private float staminaMultLowBound = 0.5f;
    private float staminaToReachLowBound = 50;

    private BoxCollider deadCollider;


	void Awake ()
    {
        tm = GameObject.FindWithTag("TeamManager").GetComponent<TeamManager>();
        animController = GetComponent<PlayerAnimationController>();
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
        InvokeRepeating("RegenerateEnergy", 1.0f, energyRegenRateInSeconds);
        attributes = GetComponent<CharacterAttributes>();
        deadCollider = transform.FindChild("DeadHitBox").GetComponent<BoxCollider>();
	}
	

	void Update ()
    {
        //debug key, currently hurts ALL players
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10);
        }
        //Debug.Log(currentEnergy);
		
	}

    public void TakeDamage (float amount)
    {
        float staminaMultiplier = staminaMultLowBound;
        if (attributes.TotalStamina < staminaToReachLowBound)
            staminaMultiplier = (attributes.TotalStamina)*((staminaMultLowBound - 1)/(staminaToReachLowBound)) + 1; //Maps 0-50 stamina to 1.0-0.5 multiplier for incoming damage
        float adjustedAmount = amount * staminaMultiplier;
        Debug.Log(staminaMultiplier + " <--- stamina multiplier");
        currentHealth -= Mathf.Abs(adjustedAmount);
        Debug.Log("Player Lost " + adjustedAmount.ToString() + " Health");
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
        animController.AnimateDeath();
        deathHandled = true;
        gameObject.GetComponent<Strategy>().setAsDead();

        // Disable collider so enemies don't see player anymore
        gameObject.GetComponent<Collider>().enabled = false;
        deadCollider.enabled = true;

       //Destroy(gameObject, 5.0f);
        Debug.Log("Player died!");
        tm.UpdateDeathCount();
        // Animation stuff goes here
    }

    public void Revive()
    {
        animController.AnimateRevive();
        deathHandled = false;
        gameObject.GetComponent<Strategy>().setAsCoopAI();
        gameObject.GetComponent<Collider>().enabled = true;
        deadCollider.enabled = false;
        currentHealth = 1;
    }

}
