﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttributes : MonoBehaviour, IAttributes {

    public int Level { get { return level; } set { level = value; } }
    public int Experience
    {
        get
        {
            return experience;
        }
        set
        {
            experience = value;
            if(experience >= experienceNeededForNextLevel)
            {
                this.LevelUp();
            }
        }
    }
    public int Strength { get { return strength; } set { strength = value; } }
    public int Intelligence { get { return intelligence; } set { intelligence = value; } }
    public int Stamina { get { return stamina; } set { stamina = value; } }

    public int PassiveStrength { get; set; }
    public int PassiveIntelligence { get; set; }
    public int PassiveStamina { get; set; }

    public int TotalStrength { get { return strength + PassiveStrength; } }
    public int TotalIntelligence { get { return intelligence + PassiveIntelligence; } }
    public int TotalStamina { get { return stamina + PassiveStamina; } }

    int level;
    int experience;
    public int experienceNeededForNextLevel;
    int experienceMultiplier;
    public int StatPoints;
    int strength;
    int intelligence;
    int stamina;


    private Object levelUpAura;
    private Object levelUpBuff;
    private AbilityAlert alert;

    public void Awake()
    {
        level = 1;
        experience = 0;
        experienceNeededForNextLevel = 10;
        experienceMultiplier = 10;
        StatPoints = 5;
        strength = 1;
        intelligence = 1;
        stamina = 1;
        PassiveStrength = 0;
        PassiveIntelligence = 0;
        PassiveStamina = 0;

        levelUpAura = Resources.Load("Levelup/LevelAura");
        levelUpBuff = Resources.Load("Levelup/LevelBuff");
        alert = GameObject.Find("AbilityAlert").GetComponent<AbilityAlert>();
    }

    void LevelUp()
    {
        level += 1;
        experience -= experienceNeededForNextLevel;
        experienceNeededForNextLevel = level * experienceMultiplier;
        StatPoints += 3;
        // strength += 1;
        // intelligence += 1;
        // stamina += 1;
        GameObject aura = Instantiate(levelUpAura, transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject;
        aura.GetComponent<StayWithPlayer>().player = transform;
        GameObject buff = Instantiate(levelUpBuff, transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject;
        buff.GetComponent<StayWithPlayer>().player = transform;
        Destroy(aura, 1.2f);
        Destroy(buff, 1.0f);
        alert.customMessage("Level up!", Color.yellow, 3.0f);
    }

    public void ResetPassiveBonus()
    {
        PassiveStrength = 0;
        PassiveIntelligence = 0;
        PassiveStamina = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StatPoints += 10;
        }
    }
}