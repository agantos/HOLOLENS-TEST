using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ScriptTesting : MonoBehaviour
{
    //Init Singletons
    FromJSONtoEngine fromJSONtoEngineInstance = FromJSONtoEngine.GetInstance();
    JSONParser JSONParserInstance = JSONParser.GetInstance();
    Dictionary<string, BaseCharacterPreset> basePresets;

    //Variables
    public JSONBaseCharacterPresetsWrapper basePresetsWrapper;
    public CharacterStatistics stats;
    public BaseCharacterPreset human;
  
    void Start()
    {        
        basePresetsWrapper = JSONParser.ParseBaseCharacterPresets("JSONs/StatisticsExample");
        //human = FromJSONtoEngine.CreateBaseCharacterPreset(basePresetsWrapper.baseCharacterPresets[0]);
        //human.stats.CalculateAllStats();
        //Debug.Log(human.stats.ToString());

        basePresets = FromJSONtoEngine.BaseCharacterPresetsTranslation(basePresetsWrapper.baseCharacterPresets);
        foreach(BaseCharacterPreset preset in basePresets.Values)
        {
            Debug.Log(preset.ToString());
        }

    }

}
