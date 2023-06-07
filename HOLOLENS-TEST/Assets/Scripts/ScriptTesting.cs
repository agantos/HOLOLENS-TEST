using Microsoft.MixedReality.Toolkit.Experimental.SpatialAwareness;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScriptTesting : MonoBehaviour
{
    ////Initialize Singletons
    //FromJSONtoEngine fromJSONtoEngineInstance = FromJSONtoEngine.GetInstance();
    //JSONParser JSONParserInstance = JSONParser.GetInstance();
    //EffectSucceedsChecker effectSuccessChecker = EffectSucceedsChecker.GetInstance();
    //AbilityManager abilityManager = AbilityManager.GetInstance();

    //Variables
    public static Dictionary<string, BaseCharacterPreset> basePresetPool;
    public static Dictionary<string, Character> characterPool;
    public JSONBaseCharacterPresetsWrapper parsedBasePresets;

    public JSONAbilities jsonAbilities;

    public JSONCharacters JSONcharacters;
    void InitializeSingletons()
    {
        JSONClass_to_EngineClass fromJSONtoEngineInstance = JSONClass_to_EngineClass.GetInstance();
        JSONFile_to_JSONClass JSONParserInstance = JSONFile_to_JSONClass.GetInstance();
        EffectSucceedsChecker effectSuccessChecker = EffectSucceedsChecker.GetInstance();
        AbilityManager abilityManager = AbilityManager.GetInstance();
    }

    void LoadFromJsons()
    {
        parsedBasePresets = JSONFile_to_JSONClass.ParseBaseCharacterPresets("JSONs/BaseCharacterPresets");
        basePresetPool = JSONClass_to_EngineClass.CreateBaseCharacterPresetDictionary(parsedBasePresets.baseCharacterPresets);

        jsonAbilities = JSONFile_to_JSONClass.ParseAbilities("JSONs/Abilities");
        JSONClass_to_EngineClass.CreateAbilitiesDictionary(jsonAbilities);

        JSONcharacters = JSONFile_to_JSONClass.ParseCharacters("JSONs/CharacterExample");
        JSONClass_to_EngineClass.FillCharacterPool(JSONcharacters);            
    }
    void Start()
    {
        InitializeSingletons();
        LoadFromJsons();
        CreateCharacters();       
    }


    void CreateCharacters()
    {
        foreach(Character c in characterPool.Values)
        {
            c.LoadCharacterBasicPresetsFromPool(basePresetPool);
            c.GetStats().CalculateAllStats();
            //Debug.Log(c.ToString("  "));
        }
    }
}
