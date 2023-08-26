using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class OngoingEffect
{
    public string name;
    public int duration;
    public EffectStats effect;
    public Character attacker;
}

public class Character
{
    //Basics
    public string name;
    CharacterStats stats = new CharacterStats();

    //Presets
    public List<string> basePresets = new List<string>();
    public List<string> additionalPresets = new List<string>();

    //TurnEconomy
    public Dictionary<string, int> turnEconomy;
    public Dictionary<string, int> currentTurnEconomy;

    //Ability Related
    public Dictionary<string, string> abilities = new Dictionary<string, string>();
    public List<OngoingEffect> ongoingEffects = new List<OngoingEffect>();
    

    //Loading Character Methods
    public void LoadCharacterBasicPresetsFromPool(Dictionary<string, BaseCharacterPreset> basePresetsPool)
    {
        foreach (string presetName in basePresets)
        {
            basePresetsPool[presetName].AddPresetToCharacter(this);
        }
    }

    public void AddStat(CharacterStat stat) { 
        stats.GetStatistics().Add(stat.GetName(), stat.Clone()); 
    }   
   
    //Ability Activation Methods
    public void ActivateAbility(string abilityName, out List<bool> abilitySuccessList, List<Character> defenders = null, Character attacker = null)
    {

        abilitySuccessList = new List<bool>();

        if(abilities.TryGetValue(abilityName, out abilityName))
        {
            foreach(Character defender in defenders)
            {
                bool succeeds;
                AbilitiesManager.ApplyAbilityEffect(abilityName, out succeeds, defender, attacker);
                abilitySuccessList.Add(succeeds);
            }
                

            AbilitiesManager.ApplyAbilityCost(abilityName, attacker);
        }
        else
        {
            Assert.IsFalse(true, "Character " + name + " does not have the ability " + abilityName);
        }
    }
    
    //Turn Change Methods
    public void RefreshTurnEconomy()
    {
        foreach(string name in turnEconomy.Keys)
        {
            currentTurnEconomy[name] = turnEconomy[name];
        }
    }

    public void OnStartTurn()
    {
        Debug.Log("Defense of " + name + " is " + GetStat("Defense").GetCurrentValue());

        //Refresh the turn economy
        RefreshTurnEconomy();

        //Heal speed damage that is applied on movement
        CharacterStat speed = GetStat("Speed");
        speed.HealDamage(speed.GetDamage());
        speed.CalculateCurrentValue();

        //Apply Ongoing Effects
        foreach(OngoingEffect ongoing in ongoingEffects)
        {
            ongoing.effect.ApplyEffect(true, this, ongoing.attacker);

            ongoing.duration -= 1;            
            if(ongoing.duration == 0)
                ongoingEffects.Remove(ongoing);
        }

        //Temporal Effects
        stats.UpdateTemporalEffects();
    }



    //Get Methods
    public CharacterStats GetStats() { return stats; }
    public CharacterStat GetStat(string name)
    {
        CharacterStat stat = stats.GetStat(name);
        Assert.IsNotNull(stat, "No stat with name = " + name + " exists.");
        return stats.GetStat(name);
    }
    public Ability GetCharacterAbility(string name)
    {
        return AbilitiesManager.abilityPool[name];
    }

    //Misc Methods
    public string ToString(string prevTab)
    {
        string tab = "  ";
        string currTab = tab + prevTab;
        string s = prevTab + "Character: " + this.name + "\n";

        s += stats.ToString(currTab) + "\n";

        s += currTab + "TurnEconomy: [  ";
        foreach (string actionName in turnEconomy.Keys)
            s += currTab + tab + actionName + " " + turnEconomy[actionName] + "\n";

        s += currTab + "Abilities: [  ";
        foreach (string ability in abilities.Values)
        {
            s += ability + "  |  ";
        }
        s += "]\n";

        s += currTab + "BasePresets: [  ";
        foreach (string preset in basePresets)
        {
            s += preset + "  |  ";
        }
        s += "]";

        return s;
    }
}