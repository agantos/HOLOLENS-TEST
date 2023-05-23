using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A singleton class that takes json filepaths and
//  1. parses the json files
//  2. returns the JSONClasses filled with the parsed data
public class JSONParser
{
    private JSONParser() {}
    private static JSONParser instance = null;

    public static JSONBaseCharacterPresetWrapper presetWrapper;
    public static JSONBaseCharacterPresetsWrapper presetsWrapper;

    public static JSONParser GetInstance()
    {
        if (instance == null)
        {
            instance = new JSONParser();
        }
        return instance;
    }
    public static JSONBaseCharacterPresetWrapper ParseBaseCharacterPreset(string jsonPath)
    {
        JSONBaseCharacterPresetWrapper wrapper;
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonPath);
        wrapper = JsonUtility.FromJson<JSONBaseCharacterPresetWrapper>(jsonFile.text);
        return wrapper;
    }
    public static JSONBaseCharacterPresetsWrapper ParseBaseCharacterPresets(string jsonPath) {
        JSONBaseCharacterPresetsWrapper wrapper;
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonPath);
        wrapper = JsonUtility.FromJson<JSONBaseCharacterPresetsWrapper>(jsonFile.text);
        return wrapper;
    }
    public static JSONEffect ParseEffect(string jsonPath)
    {
        JSONEffect wrapper;
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonPath);
        wrapper = JsonUtility.FromJson<JSONEffect>(jsonFile.text);
        return wrapper;
    }
    public static JSONAbility ParseAbility(string jsonPath)
    {
        JSONAbility wrapper;
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonPath);
        wrapper = JsonUtility.FromJson<JSONAbility>(jsonFile.text);
        return wrapper;
    }

}

//A singleton class that takes JSONClasses and forms the engine classes
public class FromJSONtoEngine
{
    private FromJSONtoEngine(){}
    private static FromJSONtoEngine instance = null;
    public static FromJSONtoEngine GetInstance()
    {
        if (instance == null)
        {
            instance = new FromJSONtoEngine();
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
        preset.SetName(JSONpreset.name);
        return preset;
    }

    //Create PrimaryEffectStats from the class created from the parsed JSON.
    //  -The field followUpEffects is empty.
    public static PrimaryEffectStats CreatePrimaryEffectSkeleton(JSONEffect jsonEffect)
    {
        PrimaryEffectStats effect = new PrimaryEffectStats();

        //Targetting statistics
        if(jsonEffect.targetting != null)
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
        effect.CreateDamageStatistics(jsonEffect.damage.amount.baseAmount, jsonEffect.damage.amount.statsAffecting, jsonEffect.damage.damagedStat);

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
        effect.CreateDamageStatistics(jsonEffect.damage.amount.baseAmount, jsonEffect.damage.amount.statsAffecting, jsonEffect.damage.damagedStat);
        
        return effect;
    }

    //Create Ability from the class created from the parsed JSON
    public static List<PrimaryEffectStats> CreatePrimaryEffects(JSONEffect[] jsonEffects)
    {
        List<PrimaryEffectStats> effectsList = new List<PrimaryEffectStats>();
        foreach(JSONEffect jsonEffect in jsonEffects)
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
        return new Ability(jsonAbility.name, jsonAbility.description, CreatePrimaryEffects(jsonAbility.effects));
    }

    //Create a dictionary of BaseCharacterPresets from the parsed JSON 
    public static Dictionary<string, BaseCharacterPreset> BaseCharacterPresetsTranslation(JSONBaseCharacterPreset[] basePresets)
    {
        Dictionary<string, BaseCharacterPreset> list = new Dictionary<string, BaseCharacterPreset>();
        foreach (JSONBaseCharacterPreset preset in basePresets)
        {
            list.Add(preset.name, CreateBaseCharacterPreset(preset));
        }

        return list;
    }
}