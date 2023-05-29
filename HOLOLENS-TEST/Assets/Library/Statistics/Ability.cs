using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability
{
    public string name;
    public string description;
    public List<PrimaryEffectStats> effects;

    public Ability(string name, string description, List<PrimaryEffectStats> effects)
    {
        this.name = name;
        this.description = description;
        this.effects = effects;
    }
}

