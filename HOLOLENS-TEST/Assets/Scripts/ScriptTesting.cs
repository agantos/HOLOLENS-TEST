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
    Dictionary<string, BaseCharacterPreset> basePresets;
    public JSONBaseCharacterPresetsWrapper parsedBasePresets;

    public JSONAbility jsonAbility;
    public Ability charismaAttackAbility;

    public JSONAbilities jsonAbilities;

    public JSONCharacter JSONjosh;
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
        basePresets = FromJSONtoEngine.CreateBaseCharacterPresetDictionary(parsedBasePresets.baseCharacterPresets);

        jsonAbility = JSONParser.ParseAbility("JSONs/Ability");
        charismaAttackAbility = FromJSONtoEngine.CreateAbility(jsonAbility);

        jsonAbilities = JSONParser.ParseAbilities("JSONs/Abilities");
        FromJSONtoEngine.CreateAbilitiesDictionary(jsonAbilities);

        JSONjosh = JSONParser.ParseCharacter("JSONs/CharacterExample");
        josh = FromJSONtoEngine.CreateCharacter(JSONjosh);
        

        //foreach(string ab in abilities.Keys)
        //{
        //    Debug.Log("Ability: " + ab);
        //}

        //foreach (PrimaryEffectStats stat in charismaAttackAbility.effects)
        //{
        //    Debug.Log(stat.ToString(""));
        //}

        //foreach (BaseCharacterPreset preset in basePresets.Values)
        //{
        //    Debug.Log(preset.ToString("  "));
        //}

        
    }
    void Start()
    {
        InitializeSingletons();
        LoadFromJsons();

        josh.LoadCharacterBasicPresets(basePresets);
        josh.GetStats().CalculateAllStats();
        Debug.Log(josh.ToString("  "));

        josh.ActivateOwnedAbility("Charisma_Melee_Attack_Poisoned", josh, josh);
    }

}
