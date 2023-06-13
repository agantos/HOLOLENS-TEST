using Microsoft.MixedReality.Toolkit.Experimental.SpatialAwareness;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    //Variables
    public static Dictionary<string, BaseCharacterPreset> basePresetPool;
    public static Dictionary<string, Character> characterPool;

    //List of GameObjects of all Characters
    public static List<GameObject> characterGameObjects = new List<GameObject>();

    void Start()
    {
        InitializeSingletons();
        LoadFromJsons();
        CreateCharacters();
    }

    void InitializeSingletons()
    {
        JSONClass_to_EngineClass fromJSONtoEngineInstance = JSONClass_to_EngineClass.GetInstance();
        JSONFile_to_JSONClass JSONParserInstance = JSONFile_to_JSONClass.GetInstance();
        EffectSucceedsChecker effectSuccessChecker = EffectSucceedsChecker.GetInstance();
        AbilityManager abilityManager = AbilityManager.GetInstance();
    }

    void LoadFromJsons()
    {
        JSONBaseCharacterPresetsWrapper parsedBasePresets = JSONFile_to_JSONClass.ParseBaseCharacterPresets("JSONs/BaseCharacterPresets");
        basePresetPool = JSONClass_to_EngineClass.CreateBaseCharacterPresetDictionary(parsedBasePresets.baseCharacterPresets);

        JSONAbilities jsonAbilities = JSONFile_to_JSONClass.ParseAbilities("JSONs/Abilities");
        JSONClass_to_EngineClass.CreateAbilitiesDictionary(jsonAbilities);

        JSONCharacters JSONcharacters = JSONFile_to_JSONClass.ParseCharacters("JSONs/CharacterExample");
        JSONClass_to_EngineClass.FillCharacterPool(JSONcharacters);            
    }

    void CreateCharacters()
    {
        foreach(Character c in characterPool.Values)
        {
            c.LoadCharacterBasicPresetsFromPool(basePresetPool);
            c.GetStats().CalculateAllStats();
        }
    }
}
