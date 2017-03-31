using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {

    public float maxHealth = 100;
    public int experiencePoints;

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
    private TeamManager teamManager;

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
        teamManager = GameObject.FindGameObjectWithTag("TeamManager").GetComponent<TeamManager>();
    }

    void Update()
    {
        if (isDamaged && !isDead)
            ShowHealthBar();
    }

    public void TakeDamage(float amount)
    {
        amount = Mathf.Abs(amount);
        currentHealth -= amount;
        healthBar.fillAmount = currentHealth / maxHealth;

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
        teamManager.RemoveDeadEnemy(gameObject);
        animator.AnimateDeath();
        teamManager.AwardExperience(experiencePoints);
        HideHealthBar();
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
        
        newDmg.transform.localRotation = damageTextTrans.localRotation;
        newDmg.transform.localScale = damageTextTrans.localScale * (0.5f + Mathf.Clamp((2 * damage) / maxHealth, 0, 1.25f));
        newDmg.transform.localPosition = damageTextTrans.localPosition + (15 * newDmg.transform.localScale);
        Text txt = newDmg.GetComponent<Text>();
        txt.color = new Color(1, Mathf.Clamp((255 - (2 * damage * (maxHealth / 255))), 0, 255) / 255, 0, 1);
        txt.text = ((int)damage).ToString();
        newDmg.SetActive(true);
    }
}
