using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

//A singleton class that takes JSONClasses and forms the engine classes
public class JSONClass_to_EngineClass
{
    private JSONClass_to_EngineClass() { }
    private static JSONClass_to_EngineClass instance = null;
    public static JSONClass_to_EngineClass GetInstance()
    {
        if (instance == null)
        {
            instance = new JSONClass_to_EngineClass();
        }
        return instance;
    }

    private static void CreateStatRelation(string input, out string statName, out StatFunctor fun)
    {
        float value;
        string op;
        // Split the input string by the separator "|"
        string[] parts = input.Split('|');

        // Get the operator part
        op = parts[0].Trim();

        // Get the value part and parse it to a float
        if (!float.TryParse(parts[1].Trim(), out value))
        {
            Debug.LogError("Parsing Relation Stat Error: This does not contain a float");
        }

        // Get the whatever part
        statName = parts[2].Trim();
        fun = new StatFunctor(op, value);
    }

    //Creation of CharacterStat from JSONstat is done in 2 steps    
    private static CharacterStatistics CreateCharacterStats(JSONstat[] statistics)
    {
        CharacterStatistics stats = new CharacterStatistics();
        //  1. Create all CharacterStats with name and staticValue
        foreach (JSONstat stat in statistics)
        {
            stats.AddStat(stat.name, stat.staticValue);
        }

        //  2. Fill each statRelation field with a reference to each CharacterStat needed.
        foreach (JSONstat stat in statistics)
        {
            if (stat.statRelations.Length > 0)
            {
                foreach (string statRelation in stat.statRelations)
                {
                    string affectorStatName;
                    StatFunctor fun;
                    CreateStatRelation(statRelation, out affectorStatName, out fun);
                    stats.AddStatRelation(stat.name, affectorStatName, fun);
                }
            }
        }
        return stats;
    }

    //Create BaseCharacterPreset from the class created from the parsed JSON 
    public static BaseCharacterPreset CreateBaseCharacterPreset(JSONBaseCharacterPresetWrapper JSONpreset)
    {
        BaseCharacterPreset preset = new BaseCharacterPreset();
        preset.SetStats(CreateCharacterStats(JSONpreset.GetStatistics()));
        preset.SetName(JSONpreset.GetName());
        return preset;
    }

    //Create BaseCharacterPreset from the class created from the parsed JSON 
    private static BaseCharacterPreset CreateBaseCharacterPreset(JSONBaseCharacterPreset JSONpreset)
    {
        BaseCharacterPreset preset = new BaseCharacterPreset();
        preset.SetStats(CreateCharacterStats(JSONpreset.statistics));
        preset.SetAbilities(JSONpreset.abilities);
        preset.SetName(JSONpreset.name);
        return preset;
    }

    //Create PrimaryEffectStats from the class created from the parsed JSON.
    //  -The field followUpEffects is empty.
    public static PrimaryEffectStats CreatePrimaryEffectSkeleton(JSONEffect jsonEffect)
    {
        PrimaryEffectStats effect = new PrimaryEffectStats();

        //Targetting statistics
        if (jsonEffect.targetting != null)
            effect.CreateTargetting(jsonEffect.targetting.type, jsonEffect.targetting.number);

        //Area of Effect statistics
        if (jsonEffect.areaOfEffect != null)
            effect.CreateAreaOfEffect(jsonEffect.areaOfEffect.range, jsonEffect.areaOfEffect.shape, jsonEffect.areaOfEffect.radius);

        //Success Statistics
        JSONEffectComparisonStat jsonAttacker;
        JSONEffectComparisonStat jsonDefender;

        ComparisonStat attacker = null;
        ComparisonStat defender = null;

        if (jsonEffect.succeedsOn.comparisonStats.attackerStat != null)
        {
            jsonAttacker = jsonEffect.succeedsOn.comparisonStats.attackerStat;
            attacker = new ComparisonStat(jsonAttacker.name, jsonAttacker.multiplier, jsonAttacker.bonus);
        }
        if (jsonEffect.succeedsOn.comparisonStats.defenderStat != null)
        {
            jsonDefender = jsonEffect.succeedsOn.comparisonStats.defenderStat;
            defender = new ComparisonStat(jsonDefender.name, jsonDefender.multiplier, jsonDefender.bonus);
        }

        effect.CreateEffectSucceedsStats(jsonEffect.succeedsOn.type, jsonEffect.succeedsOn.againstStatic, attacker, defender);

        //Damage Statistics
        effect.CreateDamageStatistics(jsonEffect.damage.amount.baseAmount, jsonEffect.damage.amount.statsAffecting, jsonEffect.damage.damagedStat, jsonEffect.damage.onSavedMultiplier);

        //Follow-Up Effects
        return effect;
    }

    public static FollowupEffectStats CreateFollowUpEffect(JSONEffect jsonEffect)
    {
        FollowupEffectStats effect = new FollowupEffectStats();

        //Success Statistics
        JSONEffectComparisonStat jsonAttacker;
        JSONEffectComparisonStat jsonDefender;

        ComparisonStat attacker = null;
        ComparisonStat defender = null;

        if (jsonEffect.succeedsOn.comparisonStats.attackerStat != null)
        {
            jsonAttacker = jsonEffect.succeedsOn.comparisonStats.attackerStat;
            attacker = new ComparisonStat(jsonAttacker.name, jsonAttacker.multiplier, jsonAttacker.bonus);
        }
        if (jsonEffect.succeedsOn.comparisonStats.defenderStat != null)
        {
            jsonDefender = jsonEffect.succeedsOn.comparisonStats.defenderStat;
            defender = new ComparisonStat(jsonDefender.name, jsonDefender.multiplier, jsonDefender.bonus);
        }

        effect.CreateEffectSucceedsStats(jsonEffect.succeedsOn.type, jsonEffect.succeedsOn.againstStatic, attacker, defender);

        //Damage Statistics
        effect.CreateDamageStatistics(jsonEffect.damage.amount.baseAmount, jsonEffect.damage.amount.statsAffecting, jsonEffect.damage.damagedStat, jsonEffect.damage.onSavedMultiplier);

        return effect;
    }

    //Create Ability from the class created from the parsed JSON
    public static List<PrimaryEffectStats> CreatePrimaryEffects(JSONEffect[] jsonEffects)
    {
        List<PrimaryEffectStats> effectsList = new List<PrimaryEffectStats>();
        foreach (JSONEffect jsonEffect in jsonEffects)
        {
            if (jsonEffect.isPrimary)
                effectsList.Add(CreatePrimaryEffectSkeleton(jsonEffect));
            else
                effectsList[effectsList.Count - 1].followUpEffects.Add(CreateFollowUpEffect(jsonEffect));
        }
        return effectsList;
    }

    public static Ability CreateAbility(JSONAbility jsonAbility)
    {
        Ability toReturn = new Ability(jsonAbility.name, jsonAbility.description, CreatePrimaryEffects(jsonAbility.effects));

        if (jsonAbility.turnEconomyCost != null)
        {
            toReturn.turnEconomyCost = new Dictionary<string, int>();
            foreach (JSONCost turnEconomyCost in jsonAbility.turnEconomyCost)
            {
                toReturn.turnEconomyCost.Add(turnEconomyCost.name, turnEconomyCost.cost);
            }
        }

        if (jsonAbility.statCost != null)
        {
            toReturn.statCost = new Dictionary<string, int>();
            foreach (JSONCost statCost in jsonAbility.statCost)
            {
                toReturn.statCost.Add(statCost.name, statCost.cost);
            }
        }
        return toReturn;
    }

    //Create a dictionary of Abilities from the parsed JSON
    public static void CreateAbilitiesDictionary(JSONAbilities jsonAbilities)
    {
        foreach (JSONAbility jsonAbility in jsonAbilities.abilities)
        {
            Ability tempAbility = CreateAbility(jsonAbility);
            AbilityManager.abilityPool.Add(tempAbility.name, tempAbility);
        }
    }

    //Create a dictionary of BaseCharacterPresets from the parsed JSON 
    public static Dictionary<string, BaseCharacterPreset> CreateBaseCharacterPresetDictionary(JSONBaseCharacterPreset[] basePresets)
    {
        Dictionary<string, BaseCharacterPreset> list = new Dictionary<string, BaseCharacterPreset>();
        foreach (JSONBaseCharacterPreset preset in basePresets)
        {
            list.Add(preset.name, CreateBaseCharacterPreset(preset));
        }

        return list;
    }

    public static Character CreateCharacter(JSONCharacter jsonCharacter)
    {
        Character character = new Character();

        Debug.Log(jsonCharacter.turnEconomy + "  " + jsonCharacter.name);

        character.name = jsonCharacter.name;
        character.turnEconomy = new Dictionary<string, int>();
        character.currentTurnEconomy = new Dictionary<string, int>();
        foreach (JSONTurnEconomy actionEconomy in jsonCharacter.turnEconomy)
        {
            character.turnEconomy.Add(actionEconomy.name, actionEconomy.number);
        }
        character.RefreshTurnEconomy();

        //Its optional to have character abilities
        if (jsonCharacter.abilities != null)
            foreach (string ability in jsonCharacter.abilities)
            {
                character.abilities.Add(ability, ability);
            }

        //Its mandatory to have a Base Preset
        if (jsonCharacter.basePresets == null)
        {
            Assert.IsFalse(true, "A character should always have a Base Preset");
        }
        else
            foreach (string basePresetName in jsonCharacter.basePresets)
            {
                character.basePresets.Add(basePresetName);
            }

        //Its optional to have additionaly Presets
        if (jsonCharacter.additionalPresets != null)
            foreach (string additionalPresetName in jsonCharacter.additionalPresets)
            {
                character.additionalPresets.Add(additionalPresetName);
            }

        return character;
    }

    public static void FillCharacterPool(JSONCharacters jsonCharacters)
    {
        GameManager.characterPool = new Dictionary<string, Character>();
        foreach (JSONCharacter jsonChar in jsonCharacters.characters)
        {
            Character temp = CreateCharacter(jsonChar);
            GameManager.characterPool.Add(temp.name, temp);
        }
    }
}