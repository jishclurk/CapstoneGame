using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : EnemyHealth {

    private bool deathHandled_B = false;
    private Image healthBar_B;
    private GameObject healthBarCanvas_B;
    private GameObject damageText_B;
    private Transform damageTextTrans_B;
    private BossAnimationController animator_B;
    private TeamManager teamManager_B;

    void Awake () {
        currentHealth = maxHealth;

        animator_B = GetComponent<BossAnimationController>();

        healthBarCanvas_B = transform.FindChild("EnemyHealthCanvas").gameObject;

        healthBar_B = healthBarCanvas_B.transform.FindChild("Health").GetComponent<Image>();
        healthBar_B.fillAmount = 1;
        HideHealthBar();

        damageText_B = healthBarCanvas_B.transform.FindChild("FloatingDamageText").gameObject;
        damageTextTrans_B = damageText_B.transform;
        teamManager_B = GameObject.FindGameObjectWithTag("TeamManager").GetComponent<TeamManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if (isDamaged && !isDead)
            ShowHealthBar();
    }

    public override void TakeDamage(float amount)
    {
        amount = Mathf.Abs(amount);
        currentHealth -= amount;
        healthBar_B.fillAmount = currentHealth / maxHealth;

        if (isDead && !deathHandled_B)
        {
            DisplayCombatText(amount);
            Death();
            deathHandled_B = true;
        }
        else if (!isDead)
        {
            DisplayCombatText(amount);
        }
    }

    public override void TakeStunDamage(float amount)
    {
        TakeDamage(amount);
    }

    private void Death()
    {
        // Handle death animation stuff here
        foreach (Collider col in GetComponents<Collider>())
            col.enabled = false;
        teamManager_B.RemoveDeadEnemy(gameObject);
        animator_B.AnimateDeath();
        teamManager_B.AwardExperience(experiencePoints);
        HideHealthBar();
        Destroy(gameObject, 30.0f);
    }

    private void HideHealthBar()
    {
        healthBarCanvas_B.SetActive(false);
    }

    private void ShowHealthBar()
    {
        healthBarCanvas_B.SetActive(true);
    }

    private void DisplayCombatText(float damage)
    {
        GameObject newDmg = Instantiate(damageText_B);
        newDmg.transform.SetParent(healthBarCanvas_B.transform);

        newDmg.transform.localRotation = damageTextTrans_B.localRotation;
        newDmg.transform.localScale = damageTextTrans_B.localScale * (0.5f + Mathf.Clamp(damage / 50f, 0, 1.25f));
        newDmg.transform.localPosition = damageTextTrans_B.localPosition + (15 * newDmg.transform.localScale);
        Text txt = newDmg.GetComponent<Text>();
        txt.color = new Color(1, Mathf.Clamp(1 - (damage / 50f), 0, 1), 0, 1);
        txt.text = ((int)damage).ToString();
        newDmg.SetActive(true);
    }
}
