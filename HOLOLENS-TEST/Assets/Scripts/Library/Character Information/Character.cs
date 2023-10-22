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

    //UI Related
    public Character_UI_Information character_UI_info;

    //Multiplayer Related
    public int player;

    //Initialize Character
    public void Initialize(Dictionary<string, BaseCharacterPreset> basePresetPool, Dictionary<string, AdditionalCharacterPreset> additionalPresetPool)
    {
        //Load Presets
        LoadBaseCharacterPresets(basePresetPool);
        LoadAdditionalCharacterPresets(additionalPresetPool);

        // Calculate stats
        // To do so, first set character relation stats to refer to this character
        stats.SetCharacterInCharacterRelations(this);
        stats.CalculateAllStats();

        //Load UI Information
        CreateCharacter_UI_Info();

        //Injected Rules
        RulesSpecificMethods.GetInstance().Character_onInitialize(this);
    }

    void LoadBaseCharacterPresets(Dictionary<string, BaseCharacterPreset> presetPool)
    {
        foreach (string presetName in basePresets)
        {
            presetPool[presetName].AddPresetToCharacter(this);
        }
    }

    void LoadAdditionalCharacterPresets(Dictionary<string, AdditionalCharacterPreset> presetPool)
    {
        foreach (string presetName in additionalPresets)
        {
            presetPool[presetName].AddPresetToCharacter(this);
        }
    }

    public void CreateCharacter_UI_Info()
    {
        character_UI_info = new Character_UI_Information();
        foreach (string preset in basePresets)
        {
            character_UI_info.AddPresetInformation(GameManager.GetInstance().presets_UI_Info[preset]);
        }
    }

    public void AddStat(CharacterStat stat) { 
        stats.GetStatistics().Add(stat.GetName(), stat.Clone()); 
    }   
   
    //Creates the application the data for the ability
    public void GetAbilityApplicationData(string abilityName, out List<bool> abilitySucceedsOnDefendersList, 
                                out List<EffectApplicationData> applicationDataList, 
                                List<Character> defenders = null, Character attacker = null
    )
    {

        abilitySucceedsOnDefendersList = new List<bool>();
        applicationDataList = new List<EffectApplicationData>();

        if (abilities.TryGetValue(abilityName, out abilityName))
        {
            foreach(Character defender in defenders)
            {
                bool succeeds;
                applicationDataList.AddRange(AbilitiesManager.GetAbilityApplicationData(abilityName, out succeeds, defender, attacker));
                abilitySucceedsOnDefendersList.Add(succeeds);
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
        //Refresh the turn economy
        RefreshTurnEconomy();

        RulesSpecificMethods.GetInstance().Character_onStartTurn(this);

        //Apply Ongoing Effects
        foreach (OngoingEffect ongoing in ongoingEffects)
        {
            ongoing.effect.CalculateApplicationData(true, this, ongoing.attacker);

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
        if (!abilities.ContainsKey(name))
            Assert.IsFalse(true, name + " was not found");

        return AbilitiesManager.GetInstance().abilities[name];
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


public class RulesSpecificMethods{

    static RulesSpecificMethods Instance = null;
    private RulesSpecificMethods(){}

    public static RulesSpecificMethods GetInstance()
    {
        if(Instance == null)
        {
            Instance = new RulesSpecificMethods();
        }

        return Instance;
    }

    // At the start of each turn the characters:
    //  - Restore 1 power point
    //  - Gain their speed back
    public void Character_onStartTurn(Character c)
    {
        //Heal speed damage that is applied on movement
        CharacterStat speed = c.GetStat("Speed");
        speed.HealDamage(speed.GetDamage());
        speed.CalculateCurrentValue();

        //Every turn a character gets 1 power point
        CharacterStat powerPoints = c.GetStat("Power Points");
        powerPoints.HealDamage(1);
        powerPoints.CalculateCurrentValue();
    }

    //At the start of the game the characters have no powerpoints
    public void Character_onInitialize(Character c)
    {
        CharacterStat powerPoints = c.GetStat("Power Points");
        powerPoints.DealDamage(powerPoints.GetCurrentValue());
        powerPoints.CalculateCurrentValue();
    }
}