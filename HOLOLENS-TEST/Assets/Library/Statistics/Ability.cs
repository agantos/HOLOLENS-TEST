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

    public static void ActivateAbility(string name, Character defender = null, Character attacker = null)
    {
        Ability toActivate = abilityPool[name];
        foreach(PrimaryEffectStats effect in toActivate.effects)
        {
            bool primarySucceeds = EffectSucceedsChecker.GetSuccess(effect, defender, attacker);
            foreach (FollowupEffectStats followup in effect.followUpEffects)
            {
                if (primarySucceeds || followup.appliesIfPrimaryFailed)
                {
                    EffectSucceedsChecker.GetSuccess(followup, defender, attacker);
                }
            }
        }

    }


}