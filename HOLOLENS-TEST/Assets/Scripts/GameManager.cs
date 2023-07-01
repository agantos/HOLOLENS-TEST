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
    public static Dictionary<string, GameObject> characterGameObjects = new Dictionary<string, GameObject>();

    public static InitativeOrder turnOrders;
    

    void Start()
    {
        InitializeSingletons();
        LoadFromJsons();
        CreateCharacters();
        turnOrders = new InitativeOrder(characterPool);
    }

    void InitializeSingletons()
    {
        GameplayCalculatorFunctions gameplayCalculatorFunctionsInstance = GameplayCalculatorFunctions.GetInstance(gamespace);               
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

    public void GiveTurnToCharacter()
    {
        FindAnyObjectByType<MoveCharacter>().OnChangeTurn("Jarnathan");
        FindAnyObjectByType<CharacterAbilityButtons>().OnChangeTurn("Jarnathan");
    }
}

public class InitativeOrder{
    SortedDictionary<float, string> initiativeOrder = new SortedDictionary<float, string>();
    float currentPosition;

    public InitativeOrder(Dictionary<string, Character> characterPool)
    {
        InitializeInitiativeOrder(characterPool);
        Debug.Log(ToString());
    }

    public void InitializeInitiativeOrder(Dictionary<string, Character> characterPool)
    {        
        foreach(Character c in characterPool.Values)
        {
            AddToDictionary(CalculatePosition(c.GetStats()), c);
        }

        var enumerator = initiativeOrder.Keys.GetEnumerator();
        enumerator.MoveNext();
        currentPosition = enumerator.Current;
    }
    public string GetCurrentTurn()
    {
        return initiativeOrder[currentPosition];
    }

    public void AdvancePosition()
    {
        bool flag = true;
        foreach(float position in initiativeOrder.Keys)
        {
            if (flag)
                currentPosition = position;
            if (position == currentPosition)
                flag = true;
        }
    }

    void AddToDictionary(float position, Character character)
    {
        if(initiativeOrder.ContainsKey(position))
        {
            float newPosition = SolveTie(character, GameManager.characterPool[initiativeOrder[position]], position);
            AddToDictionary(newPosition, character);
        }
        else
        {
            initiativeOrder[position] = character.name;
        }
    }    

    //Change according to ruleset
    float CalculatePosition(CharacterStatistics charStats)
    {
        return GameplayCalculatorFunctions.CalculateDiceRoll("1d20");
    }

    //Change according to ruleset
    //Return the position of the entering character.
    float SolveTie(Character entering, Character existing, float position)
    {
        return position + 0.01f;
    }


    public string ToString()
    {        
        string s = "";
        foreach(float position in initiativeOrder.Keys)
        {
            s += position.ToString() + ": " + initiativeOrder[position] + "\n"; 
        }
        return s;
    }
}