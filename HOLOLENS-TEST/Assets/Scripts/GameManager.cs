using Microsoft.MixedReality.Toolkit.Experimental.SpatialAwareness;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    //Initialize Measurement
    public GameObject gamespace;

    //Variables
    public static Dictionary<string, BaseCharacterPreset> basePresetPool;
    public static Dictionary<string, Character> characterPool;
    public static Dictionary<string, Character> playingCharacterPool = new Dictionary<string, Character>();
    public static Dictionary<string, GameObject> playingCharacterGameObjects = new Dictionary<string, GameObject>();
    public static Dictionary<string, Preset_UI_Information> presets_UI_Info;

    public static TurnManager turnManager;
    

    void Start()
    {
        InitializeSingletons();
        LoadFromJsons();
        CreateCharacters();

        //Start Counting Turns
        Invoke("FirstTurn", 2.0f);
    }

    void InitializeSingletons()
    {
        GameplayCalculatorFunctions gameplayCalculatorFunctionsInstance = GameplayCalculatorFunctions.GetInstance(gamespace);               
        JSONClass_to_EngineClass fromJSONtoEngineInstance = JSONClass_to_EngineClass.GetInstance();
        JSONFile_to_JSONClass JSONParserInstance = JSONFile_to_JSONClass.GetInstance();
        EffectSucceedsChecker effectSuccessChecker = EffectSucceedsChecker.GetInstance();
        AbilitiesManager abilityManager = AbilitiesManager.GetInstance();
    }

    void LoadFromJsons()
    {
        JSONBaseCharacterPresetsWrapper parsedBasePresets = JSONFile_to_JSONClass.ParseBaseCharacterPresets("JSONs/BaseCharacterPresets");
        basePresetPool = JSONClass_to_EngineClass.CreateBaseCharacterPresetDictionary(parsedBasePresets.baseCharacterPresets);

        JSONAbilities jsonAbilities = JSONFile_to_JSONClass.ParseAbilities("JSONs/Abilities");
        JSONClass_to_EngineClass.CreateAbilitiesDictionary(jsonAbilities);

        JSONCharacters JSONcharacters = JSONFile_to_JSONClass.ParseCharacters("JSONs/CharacterExample");
        JSONClass_to_EngineClass.FillCharacterPool(JSONcharacters);

        Preset_UI_InformationTable JSONcharacter_UI_Information = JSONFile_to_JSONClass.ParseCharacterInformationUI("JSONs/Character_UI_Information");
        presets_UI_Info = JSONClass_to_EngineClass.CreateCharacter_UI_Information(JSONcharacter_UI_Information);
    
    }

    void CreateCharacters()
    {
        foreach(Character c in characterPool.Values)
        {
            c.LoadCharacterBasicPresetsFromPool(basePresetPool);
            c.GetStats().CalculateAllStats();
            c.CreateCharacter_UI_Info();
        }
    }    

    public void NextTurn()
    {
        turnManager.NextTurn();
        MonoBehaviour.FindAnyObjectByType<CharacterMover>().OnChangeTurn(GetCurrentPlayer_Name());
        SelectAbilityUIManager.GiveTurnToPlayingCharacter();
    }

    public void FirstTurn()
    {
        turnManager = new TurnManager(playingCharacterPool);
        turnManager.FirstTurn();
        SelectAbilityUIManager.GiveTurnToPlayingCharacter();
    }

    public static Character GetCurrentPlayer_Character()
    {
        return characterPool[turnManager.GetCurrentTurn_Name()];
    }

    public static string GetCurrentPlayer_Name()
    {
        return turnManager.GetCurrentTurn_Name();
    }
}


