using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttributes : MonoBehaviour, IAttributes {

    public int Level { get { return level; } set { level = value; } }
    public int Experience { get { return experience; } set { experience = value; } }
    public int Strenth { get { return strength; } set { strength = value; } }
    public int Intelligence { get { return intelligence; } set { intelligence = value; } }
    public int Stamina { get { return stamina; } set { stamina = value; } }

    int level;
    int experience;
    int strength;
    int intelligence;
    int stamina;

    public CharacterAttributes()
    {
        level = 1;
        experience = 0;
        strength = 1;
        intelligence = 1;
        stamina = 1;
    }
}
