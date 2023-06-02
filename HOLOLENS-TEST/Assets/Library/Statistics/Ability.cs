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

public class AbilityManager
{

    private static AbilityManager instance = null;
    public static Dictionary<string, Ability> abilityPool;

    private AbilityManager()
    {
        abilityPool = new Dictionary<string, Ability>();
    }

    public static AbilityManager GetInstance()
    {
        if(instance == null)
        {
            instance = new AbilityManager();
        }
        return instance;
    }

    //Calculates success of an ability and applies its damage to a character.
    public static void ActivateAbilityEffect(string name, Character defender = null, Character attacker = null)
    {        
        Ability toActivate = abilityPool[name];

        foreach (PrimaryEffectStats primaryEffect in toActivate.effects)
        {
            //Calculate primary success and damage
            bool primarySucceeds = EffectSucceedsChecker.GetSuccess(primaryEffect, defender, attacker);
            primaryEffect.DealDamage(primarySucceeds, defender, attacker);
            
            //Calculate only the appropriate follow-up effects' success and damage
            foreach (FollowupEffectStats followup in primaryEffect.followUpEffects)
            {
                if (primarySucceeds || followup.appliesIfPrimaryFailed)
                {
                    followup.DealDamage(EffectSucceedsChecker.GetSuccess(followup, defender, attacker), defender, attacker);
                }
            }
        }
    }
}