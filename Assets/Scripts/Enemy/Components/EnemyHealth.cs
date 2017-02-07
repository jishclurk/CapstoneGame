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
    private GameObject healthBarCanvas;
    private GameObject damageText;
    private Transform damageTextTrans;
    private EnemyAnimationController animator;

    void Awake()
    {
        currentHealth = maxHealth;

        animator = GetComponent<EnemyAnimationController>();

        healthBarCanvas = transform.FindChild("EnemyHealthCanvas").gameObject;

        healthBar = healthBarCanvas.transform.FindChild("Health").GetComponent<Image>();
        healthBar.fillAmount = 1;
        HideHealthBar();

        damageText = healthBarCanvas.transform.FindChild("FloatingDamageText").gameObject;
        damageTextTrans = damageText.transform;
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
        amount = Mathf.Abs(amount);
        currentHealth -= amount;
        healthBar.fillAmount = currentHealth / maxHealth;

        if (isDamaged)
            ShowHealthBar();

        if (isDead && !deathHandled)
        {
            DisplayCombatText(amount);
            Death();
            deathHandled = true;
        }
        else if (!isDead)
        {
            DisplayCombatText(amount);
        }
    }

    private void Death()
    {
        // Handle death animation stuff here
        animator.AnimateDeath();
        Destroy(gameObject, 5.0f);
    }

    private void HideHealthBar()
    {
        healthBarCanvas.SetActive(false);
    }

    private void ShowHealthBar()
    {
        healthBarCanvas.SetActive(true);
    }

    private void DisplayCombatText(float damage)
    {
        GameObject newDmg = Instantiate(damageText);
        newDmg.transform.SetParent(healthBarCanvas.transform);
        newDmg.transform.localPosition = damageTextTrans.localPosition;
        newDmg.transform.localRotation = damageTextTrans.localRotation;
        newDmg.transform.localScale = damageTextTrans.localScale;
        newDmg.GetComponent<Text>().text = ((int) damage).ToString();
        newDmg.SetActive(true);
    }
}
