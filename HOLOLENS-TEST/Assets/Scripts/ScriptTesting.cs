using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ScriptTesting : MonoBehaviour
{
    //Initialize Singletons
    FromJSONtoEngine fromJSONtoEngineInstance = FromJSONtoEngine.GetInstance();
    JSONParser JSONParserInstance = JSONParser.GetInstance();
    EffectSucceedsChecker effectSuccessChecker = EffectSucceedsChecker.GetInstance();

    //Variables
    Dictionary<string, BaseCharacterPreset> basePresets;
    public JSONBaseCharacterPresetsWrapper parsedBasePresets;

    public JSONAbility parsedAbility;
    public Ability charismaAttackAbility;

    public Character human, dwarf;

    void LoadFromJsons()
    {
        parsedBasePresets = JSONParser.ParseBaseCharacterPresets("JSONs/StatisticsExample");
        basePresets = FromJSONtoEngine.BaseCharacterPresetsTranslation(parsedBasePresets.baseCharacterPresets);

        parsedAbility = JSONParser.ParseAbility("JSONs/Ability");
        charismaAttackAbility = FromJSONtoEngine.CreateAbility(parsedAbility);

        //foreach (PrimaryEffectStats stat in charismaAttackAbility.effects)
        //{
        //    Debug.Log(stat.ToString(""));
        //}

        //foreach (BaseCharacterPreset preset in basePresets.Values)
        //{
        //    Debug.Log(preset.ToString());
        //}
    }
    void Start()
    {
        LoadFromJsons();

        basePresets["Human"].AddAbility(charismaAttackAbility);
        basePresets["Dwarf"].AddAbility(charismaAttackAbility);

        human = new Character();
        dwarf = new Character();        

        basePresets["Human"].AddPresetToCharacter(human);
        basePresets["Dwarf"].AddPresetToCharacter(dwarf);

        human.GetStats().CalculateAllStats();
        dwarf.GetStats().CalculateAllStats();

        Debug.Log(human.GetStats().ToString());

        EffectSucceedsChecker.GetSuccess(charismaAttackAbility.effects[0], human, dwarf);
    }

}
