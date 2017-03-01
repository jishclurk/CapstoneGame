using System.Collections;
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
            Debug.Log("Experience: " + experience);
            if(experience >= experienceNeededForNextLevel)
            {
                this.LevelUp();
            }
        }
    }
    public int Strength { get { return strength; } set { strength = value; } }
    public int Intelligence { get { return intelligence; } set { intelligence = value; } }
    public int Stamina { get { return stamina; } set { stamina = value; } }

    int level;
    int experience;
    int experienceNeededForNextLevel;
    int experienceMultiplier;
    public int StatPoints;
    int strength;
    int intelligence;
    int stamina;

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
        Debug.Log("level = " + level);
        Debug.Log("experienceNeededForNextLevel = " + experienceNeededForNextLevel);
    }
}