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
    public JSONEffect parsedEffect;
    public PrimaryEffectStats charismaAttack;

    public JSONAbility parsedAbility;
    public Ability charismaAttackAbility;

    void Start()
    {
        parsedBasePresets = JSONParser.ParseBaseCharacterPresets("JSONs/StatisticsExample");
        basePresets = FromJSONtoEngine.BaseCharacterPresetsTranslation(parsedBasePresets.baseCharacterPresets);

        parsedEffect = JSONParser.ParseEffect("JSONs/Effect");
        charismaAttack = FromJSONtoEngine.CreatePrimaryEffectSkeleton(parsedEffect);

        parsedAbility = JSONParser.ParseAbility("JSONs/Ability");
        charismaAttackAbility = FromJSONtoEngine.CreateAbility(parsedAbility);

        foreach(PrimaryEffectStats stat in charismaAttackAbility.effects)
        {
            Debug.Log(stat.ToString(""));
        }

        foreach (BaseCharacterPreset preset in basePresets.Values)
        {
            Debug.Log(preset.ToString());
        }
    }

}
