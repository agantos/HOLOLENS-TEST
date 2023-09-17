using Microsoft.MixedReality.Toolkit.Experimental.SpatialAwareness;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.AI;

using Photon.Pun;

public class GameManager : MonoBehaviour
{
    //Initialize Measurement
    public GameObject gamespace;

    //Variables
    public Dictionary<string, BaseCharacterPreset> basePresetPool;
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
        Debug.Log(PhotonNetwork.IsMasterClient);

        InitializeSingletons();
        LoadFromJsons();
        CreateCharacters();
    }

    void InitializeSingletons()
    {
        GameplayCalculatorFunctions gameplayCalculatorFunctionsInstance = GameplayCalculatorFunctions.GetInstance(gamespace);               
        JSONClass_to_EngineClass fromJSONtoEngineInstance = JSONClass_to_EngineClass.GetInstance();
        JSONFile_to_JSONClass JSONParserInstance = JSONFile_to_JSONClass.GetInstance();
        EffectSucceedsChecker effectSuccessChecker = EffectSucceedsChecker.GetInstance();
        AbilitiesManager abilityManager = AbilitiesManager.GetInstance();
        AbilityDisplayGeneralMethods abilityDisplayGeneralMethods = AbilityDisplayGeneralMethods.GetInstance();
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
        CharacterMover.Instance.OnChangeTurn(GetCurrentPlayer_Name());
        SelectAbilityUIManager.Instance.GiveTurnToPlayingCharacter();

        //Tell the other players to progress in the turn order
        MultiplayerTurnManagementCalls.Instance.Propagate_NextTurn();
    }

    //Only difference is that it does not send any message in the network
    public void NextTurn_Remotely()
    {
        turnManager.NextTurn();
        CharacterMover.Instance.OnChangeTurn(GetCurrentPlayer_Name());
        SelectAbilityUIManager.Instance.GiveTurnToPlayingCharacter();
    }

    public void FirstTurn()
    {
        turnManager = new TurnManager(playingCharacterPool);
        turnManager.FirstTurn();
        hasSetInitiative = true;
        SelectAbilityUIManager.Instance.GiveTurnToPlayingCharacter();
        CharacterMover.Instance.OnChangeTurn(GetCurrentPlayer_Name());
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