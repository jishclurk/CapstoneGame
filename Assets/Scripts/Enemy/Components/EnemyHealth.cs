using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {

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

    public bool isDead { get { return currentHealth <= 0; } }
    public bool isDamaged { get { return currentHealth < maxHealth; } }

    private bool deathHandled = false;
    private Image healthBar;
    private GameObject healthBarObj;

    void Awake()
    {
        currentHealth = maxHealth;
        
        healthBarObj = transform.FindChild("HealthBar").gameObject;
        healthBar = healthBarObj.transform.FindChild("Health").GetComponent<Image>();
        healthBar.fillAmount = 1;
        HideHealthBar();
    }

    void Update()
    {
        // Just for testing purposes
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
            Debug.Log("Enemy Damaged");
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= Mathf.Abs(amount);
        if (isDead && !deathHandled)
        {
            Death();
        }

        healthBar.fillAmount = currentHealth / maxHealth;

        if (isDamaged)
            ShowHealthBar();
    }

    private void Death()
    {
        // Handle death animation stuff here
    }

    private void HideHealthBar()
    {
        healthBarObj.SetActive(false);
    }

    private void ShowHealthBar()
    {
        healthBarObj.SetActive(true);
    }
}
