﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {

    public float maxHealth = 100;
    public int experiencePoints;

    [HideInInspector]
    public float _currentHealth;
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

    private float burnDamage = 0.0f;
    private float burnTickSeconds = 1.0f;

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
        StartCoroutine(ApplyBurnDamage());
    }

    void Update()
    {
        if (isDamaged && !isDead)
            ShowHealthBar();
    }

    public virtual void TakeDamage(float amount)
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

    public void TakeBurnDamage(float amount)
    {
        burnDamage += amount;
    }

    public virtual void TakeStunDamage(float amount)
    {
        TakeDamage(amount);
        animator.AnimateStun();
    }

    private void Death()
    {
        // Handle death animation stuff here
        foreach (Collider col in GetComponents<Collider>())
            col.enabled = false;
        teamManager.RemoveDeadEnemy(gameObject);
        animator.AnimateDeath();
        teamManager.AwardExperience(experiencePoints);
        HideHealthBar();
        Destroy(gameObject, 3.0f);
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
        newDmg.transform.localScale = damageTextTrans.localScale * (0.5f + Mathf.Clamp(damage / 50f, 0, 1.25f));
        newDmg.transform.localPosition = damageTextTrans.localPosition + (15 * newDmg.transform.localScale);
        Text txt = newDmg.GetComponent<Text>();
        txt.color = new Color(1, Mathf.Clamp(1 - (damage / 50f), 0, 1), 0, 1);
        txt.text = ((int)damage).ToString();
        newDmg.SetActive(true);
    }

    private IEnumerator ApplyBurnDamage()
    {
        float lastBurnDamage = 0.0f;
        int numTicksDuration = 5;
        int tickCheck = 0;
        while (true)
        {
            yield return new WaitForSeconds(burnTickSeconds);
            if (burnDamage > 0)
            {
                TakeDamage(burnDamage);
                if (burnDamage > lastBurnDamage)
                {
                    lastBurnDamage = burnDamage;
                    tickCheck = numTicksDuration;
                }

                tickCheck -= 1;
                if (tickCheck <= 0)
                {
                    burnDamage = 0;
                    lastBurnDamage = 0;
                }
            }
        }
    }
}
