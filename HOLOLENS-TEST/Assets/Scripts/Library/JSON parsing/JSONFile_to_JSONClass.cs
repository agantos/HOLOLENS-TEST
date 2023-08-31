using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// A singleton class that takes json filepaths and
//  1. parses the json files
//  2. returns the JSONClasses filled with the parsed data
public class JSONFile_to_JSONClass
{
    private JSONFile_to_JSONClass() {}
    private static JSONFile_to_JSONClass instance = null;

    public static JSONBaseCharacterPresetWrapper presetWrapper;
    public static JSONBaseCharacterPresetsWrapper presetsWrapper;

    public static JSONFile_to_JSONClass GetInstance()
    {
        if (instance == null)
        {
            instance = new JSONFile_to_JSONClass();
        }
        return instance;
    }
    
    public static JSONBaseCharacterPresetsWrapper ParseBaseCharacterPresets(string jsonPath) {
        JSONBaseCharacterPresetsWrapper wrapper;
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonPath);
        wrapper = JsonUtility.FromJson<JSONBaseCharacterPresetsWrapper>(jsonFile.text);
        return wrapper;
    }
    public static JSONAbilities ParseAbilities(string jsonPath)
    {
        JSONAbilities wrapper;
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonPath);
        wrapper = JsonUtility.FromJson<JSONAbilities>(jsonFile.text);
        return wrapper;
    }
    public static JSONCharacters ParseCharacters(string jsonPath)
    {
        JSONCharacters wrapper;
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonPath);
        wrapper = JsonUtility.FromJson<JSONCharacters>(jsonFile.text);
        return wrapper;
    }

    public static Preset_UI_InformationTable ParseCharacterInformationUI(string jsonPath)
    {
        Preset_UI_InformationTable wrapper;
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonPath);
        wrapper = JsonUtility.FromJson<Preset_UI_InformationTable>(jsonFile.text);
        return wrapper;
    }
}

