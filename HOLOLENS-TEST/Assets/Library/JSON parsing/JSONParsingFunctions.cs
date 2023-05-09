using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JSONstat
{
    public string name;
    public int staticValue;
    public string[] statRelations;
}

[System.Serializable]
public class JSONstats
{
    public JSONstat[] statistics;
}

[System.Serializable]
public class JSONstatsEncapsulator
{
    public JSONstats JSONstats;
    public void Parse(string jsonPath)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonPath);
        JSONstats = JsonUtility.FromJson<JSONstats>(jsonFile.text);
    }
}

[System.Serializable]
public class JSONBaseCharacterPreset
{
    public string name;
    public JSONstat[] statistics;
}

[System.Serializable]
public class JSONBaseCharacterPresetWrapper
{
    public JSONBaseCharacterPreset baseCharacterPreset;

    public JSONstat[] GetStatistics() { return baseCharacterPreset.statistics; }
    public string GetName() { return baseCharacterPreset.name; }
}

// A singleton class that takes json filepaths and
//  1. parses the json files
//  2. returns the JSONClasses filled with the parsed data
public class JSONParser
{
    private JSONParser() { }
    private static JSONParser instance = null;
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

    public static void CreateStatRelation(string input, out string statName, out StatFunctor fun)
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
    public static CharacterStatistics CreateCharacterStats(JSONstat[] statistics)
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

    public static BaseCharacterPreset CreateBaseCharacterPreset(JSONBaseCharacterPresetWrapper JSONpreset)
    {
        BaseCharacterPreset preset= new BaseCharacterPreset();
        preset.SetStats(CreateCharacterStats(JSONpreset.GetStatistics()));
        preset.SetName(JSONpreset.GetName());
        return preset;
    }
}