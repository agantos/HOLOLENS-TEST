using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAnimationTypes
{
    public AnimationType attacker;
    public AnimationType defender_AbilitySucceeds;
    public AnimationType defender_AbilityFails;

    public AbilityAnimationTypes(AnimationType att, AnimationType def_succ, AnimationType def_fail)
    {
        attacker = att;
        defender_AbilityFails = def_fail;
        defender_AbilitySucceeds = def_succ;
    }
}

public class Ability
{
    public string name;
    public string description;
    public AbilityAnimationTypes animationTypes;

    public List<PrimaryEffectStats> effects;
    public Dictionary<string, int> turnEconomyCost;
    public Dictionary<string, int> statCost;
    public List<string> tags;

    public Ability(string name, string description, AbilityAnimationTypes animationTypes, List<PrimaryEffectStats> effects, string[] tags)
    {
        this.name = name;
        this.description = description;
        this.animationTypes = animationTypes;
        this.effects = effects;

        this.tags = new List<string>();
        foreach(string tag in tags)
        {
            this.tags.Add(tag);
        }
    }

    public string ToString(string prevTab)
    {
        string tab = "  ";
        string currTab = tab + prevTab;
        string s = prevTab + "Ability: " + this.name + "\n";
        
        s += currTab + "Turn Economy Cost: {" + "\n";        
        foreach (string economyCostName in turnEconomyCost.Keys)
        {
            s += currTab + tab + economyCostName + " " + turnEconomyCost[economyCostName] + "\n"; 
        }
        s += currTab + "}" + "\n";

        s += currTab + "Stat Cost: {" + "\n";
        foreach (string statCostName in turnEconomyCost.Keys)
        {
            s += currTab + tab + statCostName + " " + turnEconomyCost[statCostName] + "\n";
        }
        s += currTab + "}" + "\n";

        return s;
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

    //Check if a character can pay the cost of an ability
    public static bool Activate_CheckCost(string name, Character attacker)
    {        
        Ability toActivate = abilityPool[name];
        bool canActivate = true;

        //Check Turn Economy Cost
        foreach(string costName in toActivate.turnEconomyCost.Keys)
        {
            int newValue = attacker.currentTurnEconomy[costName] - toActivate.turnEconomyCost[costName];
            if (newValue < 0)
            {
                canActivate = false;
                break;
            }
        }

        if (!canActivate) return false;

        //Check Stat Cost
        foreach (string statName in toActivate.statCost.Keys)
        {
            int newValue = attacker.GetStat(statName).GetCurrentValue() - toActivate.statCost[statName];
            if (newValue < 0)
            {
                canActivate = false;
                break;
            }
        }
        return canActivate;
    }

    //Apply the cost of an ability
    public static void Activate_ApplyCost(string name, Character attacker)
    {
        Ability toActivate = abilityPool[name];
        
        //Apply Turn Economy Cost
        foreach (string costName in toActivate.turnEconomyCost.Keys)
        {            
            attacker.currentTurnEconomy[costName] -= toActivate.turnEconomyCost[costName];
        }

        //Apply Stat Cost
        foreach(string statName in toActivate.statCost.Keys)
        {
            attacker.GetStat(statName).DealDamage(toActivate.statCost[statName]);
            
            //MAY BE REMOVED TO BE ADDED ELSEWHERE
            attacker.GetStat(statName).CalculateCurrentValue();
        }
    }

    //Calculates success of an ability and applies its damage to a character.
    public static void Activate_PerformEffect(string name, out bool effectSucceeds, Character defender = null,  Character attacker = null)
    {        
        Ability toActivate = abilityPool[name];
        effectSucceeds = false;

        foreach (PrimaryEffectStats primaryEffect in toActivate.effects)
        {
            //Calculate primary success and damage
            effectSucceeds = EffectSucceedsChecker.GetSuccess(primaryEffect, defender, attacker);
            primaryEffect.DealDamage(effectSucceeds, defender, attacker);
            
            //Calculate only the appropriate follow-up effects' success and damage
            foreach (FollowupEffectStats followup in primaryEffect.followUpEffects)
            {
                if (effectSucceeds || followup.appliesIfPrimaryFailed)
                {
                    followup.DealDamage(EffectSucceedsChecker.GetSuccess(followup, defender, attacker), defender, attacker);
                }
            }
        }
    }
}