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

    public JSONAbility jsonAbility;
    public Ability charismaAttackAbility;

    public JSONAbilities jsonAbilities;

    public JSONCharacters JSONcharacters;
    public Character josh;


    void InitializeSingletons()
    {
        FromJSONtoEngine fromJSONtoEngineInstance = FromJSONtoEngine.GetInstance();
        JSONParser JSONParserInstance = JSONParser.GetInstance();
        EffectSucceedsChecker effectSuccessChecker = EffectSucceedsChecker.GetInstance();
        AbilityManager abilityManager = AbilityManager.GetInstance();
    }

    void LoadFromJsons()
    {
        parsedBasePresets = JSONParser.ParseBaseCharacterPresets("JSONs/BaseCharacterPresets");
        basePresetPool = FromJSONtoEngine.CreateBaseCharacterPresetDictionary(parsedBasePresets.baseCharacterPresets);

        jsonAbility = JSONParser.ParseAbility("JSONs/Ability");
        charismaAttackAbility = FromJSONtoEngine.CreateAbility(jsonAbility);

        jsonAbilities = JSONParser.ParseAbilities("JSONs/Abilities");
        FromJSONtoEngine.CreateAbilitiesDictionary(jsonAbilities);

        JSONcharacters = JSONParser.ParseCharacters("JSONs/CharacterExample");
        FromJSONtoEngine.FillCharacterPool(JSONcharacters);            
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
