using Microsoft.MixedReality.Toolkit.Experimental.SpatialAwareness;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.AI;

using Photon.Pun;

public class GameManager : MonoBehaviour
{
    //Initialize in UnityEditor
    public GameObject FloatingTextPrefab;

    //Initialize Measurement
    public GameObject gamespace;

    //Variables
    public Dictionary<string, BaseCharacterPreset> basePresetPool;
    public Dictionary<string, AdditionalCharacterPreset> additionalPresetPool;
    public Dictionary<string, Character> characterPool;
    public Dictionary<string, Character> playingCharacterPool = new Dictionary<string, Character>();
    public Dictionary<string, GameObject> playingCharacterGameObjects = new Dictionary<string, GameObject>();
    public Dictionary<string, Preset_UI_Information> presets_UI_Info;

    public TurnManager turnManager;

    private static GameManager Instance = null;

    //For Multiplayer
    public bool hasSetInitiative = false;
    public int player = 0;

    private GameManager() { GetInstance(); }

    public static GameManager GetInstance()
    {
        return Instance;
    }

    private void Awake()
    {
        // Ensure only one instance of GameManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManager objects
        }
    }

    void Start()
    {
        InitializeSingletons();
        LoadFromJsons();
        CreateCharacters();

        Invoke("FirstTurn", 2);
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
        //Load Character Presets
        JSONBaseCharacterPresetsWrapper parsedPresets = JSONFile_to_JSONClass.ParseBaseCharacterPresets("JSONs/BaseCharacterPresets");
        basePresetPool = JSONClass_to_EngineClass.CreateBaseCharacterPresetDictionary(parsedPresets.baseCharacterPresets);
        additionalPresetPool = JSONClass_to_EngineClass.CreateAdditionalCharacterPresetDictionary(parsedPresets.additionalCharacterPresets);

        //Load Ability Rule-Based Information
        JSONAbilities jsonAbilities = JSONFile_to_JSONClass.ParseAbilities("JSONs/Abilities");
        JSONClass_to_EngineClass.FillAbilitiesDictionary(jsonAbilities);

        //Load Ability Presentation Information
        JSONAbilityPresentations jsonAbilitiesPresentation = JSONFile_to_JSONClass.ParseAbilitiesPresentation("JSONs/AbilitiesPresentation");
        JSONClass_to_EngineClass.CreateAbilityPresentationsDictionary(jsonAbilitiesPresentation);

        //Load Character Information
        JSONCharacters JSONcharacters = JSONFile_to_JSONClass.ParseCharacters("JSONs/CharacterExample");
        JSONClass_to_EngineClass.FillCharacterPool(JSONcharacters);

        //Load Character UI Information
        Preset_UI_InformationTable JSONcharacter_UI_Information = JSONFile_to_JSONClass.ParseCharacterInformationUI("JSONs/Character_UI_Information");
        presets_UI_Info = JSONClass_to_EngineClass.CreateCharacter_UI_Information(JSONcharacter_UI_Information);
    
    }

    void CreateCharacters()
    {
        foreach(Character c in characterPool.Values)
        {
            c.Initialize(basePresetPool, additionalPresetPool);

            Debug.Log(c.name);
            Debug.Log(c.GetStats().ToString(""));
        }
    }    

    public void NextTurn()
    {
        turnManager.NextTurn();

        CharacterMoveManager.Instance.OnChangeTurn(GetCurrentPlayer_Name());
        SelectAbilityUIManager.Instance.GiveTurnToPlayingCharacter();

        //Tell the other players to progress in the turn order
        MultiplayerTurnManagementCalls.Instance.Propagate_NextTurn();

        //Move the portrait Crystal
        CharacterPortraitManager.Instance.PlaceCrystal();
    }

    //Only difference is that it does not send any message in the network
    public void NextTurn_Remotely()
    {
        turnManager.NextTurn();

        CharacterMoveManager.Instance.OnChangeTurn(GetCurrentPlayer_Name());
        SelectAbilityUIManager.Instance.GiveTurnToPlayingCharacter();

        //Move the portrait Crystal
        CharacterPortraitManager.Instance.PlaceCrystal();
    }

    public void FirstTurn()
    {
        turnManager = new TurnManager(playingCharacterPool);
        turnManager.FirstTurn();
        hasSetInitiative = true;
        SelectAbilityUIManager.Instance.GiveTurnToPlayingCharacter();
        CharacterMoveManager.Instance.OnChangeTurn(GetCurrentPlayer_Name());
        CharacterPortraitManager.Instance.Initialize();
    }

    public void FirstTurn_Remotely(float[] initiatives, string[] characters)
    {
        turnManager = new TurnManager(initiatives, characters);
        turnManager.FirstTurn();
        SelectAbilityUIManager.Instance.GiveTurnToPlayingCharacter();
        CharacterPortraitManager.Instance.Initialize();
    }

    public Character GetCurrentPlayer_Character()
    {
        return GetInstance().characterPool[GetInstance().turnManager.GetCurrentTurn_Name()];
    }

    public string GetCurrentPlayer_Name()
    {
        return GetInstance().turnManager.GetCurrentTurn_Name();
    }
}