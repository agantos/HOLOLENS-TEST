using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability
{
    public string name;
    public string description;
    public Animations animationTypes;

    public List<PrimaryEffectStats> effects;
    public Dictionary<string, int> turnEconomyCost;
    public Dictionary<string, int> statCost;
    public List<string> tags;

    public Ability(string name, string description, List<PrimaryEffectStats> effects, string[] tags)
    {
        this.name = name;
        this.description = description;
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

public class AbilitiesManager
{
    private static AbilitiesManager instance = null;
    public Dictionary<string, Ability> abilities;
    public Dictionary<string, AbilityPresentation> abilitiesPresentation;

    private AbilitiesManager()
    {
        abilities = new Dictionary<string, Ability>();
        abilitiesPresentation = new Dictionary<string, AbilityPresentation>();
    }

    public static AbilitiesManager GetInstance()
    {
        if(instance == null)
        {
            instance = new AbilitiesManager();
        }
        return instance;
    }

    //Check if a character can pay the cost of an ability
    public static bool Activate_CheckCost(string name, Character attacker)
    {        
        Ability toActivate = GetInstance().abilities[name];
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
    public static void ApplyAbilityCost(string abilityName, Character attacker)
    {
        Ability toActivate = GetInstance().abilities[abilityName];
        
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

    //Creates the application data for an ability and a single defender
    public static List<EffectApplicationData> GetAbilityApplicationData(string abilityName, out bool effectSucceeds, /*out EffectResults effectResults,*/ Character defender = null,  Character attacker = null)
    {
        List<EffectApplicationData> applicationDataList = new List<EffectApplicationData>();

        Ability toActivate = GetInstance().abilities[abilityName];
        effectSucceeds = false;

        foreach (PrimaryEffectStats primaryEffect in toActivate.effects)
        {
            //Calculate primary success and damage
            effectSucceeds = EffectSucceedsChecker.GetSuccess(primaryEffect, defender, attacker);
            applicationDataList.Add(primaryEffect.CalculateApplicationData(effectSucceeds, defender, attacker));
            
            //Calculate only the appropriate follow-up effects' success and damage
            foreach (FollowupEffectStats followup in primaryEffect.followUpEffects)
            {
                if (effectSucceeds || followup.appliesIfPrimaryFailed)
                {
                    applicationDataList.Add(
                        followup.CalculateApplicationData(
                            EffectSucceedsChecker.GetSuccess(followup, defender, attacker), defender, attacker)
                    );
                }
            }
        }

        return applicationDataList;
    }
}

