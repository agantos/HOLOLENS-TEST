using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ScriptTesting : MonoBehaviour
{
    //Initialize Singletons
    FromJSONtoEngine fromJSONtoEngineInstance = FromJSONtoEngine.GetInstance();
    JSONParser JSONParserInstance = JSONParser.GetInstance();

    //Variables
    Dictionary<string, BaseCharacterPreset> basePresets;
    public JSONBaseCharacterPresetsWrapper parsedBasePresets;

  
    void Start()
    {
        parsedBasePresets = JSONParser.ParseBaseCharacterPresets("JSONs/StatisticsExample");
        basePresets = FromJSONtoEngine.BaseCharacterPresetsTranslation(parsedBasePresets.baseCharacterPresets);

        foreach (BaseCharacterPreset preset in basePresets.Values)
        {
            Debug.Log(preset.ToString());
        }

    }

}
