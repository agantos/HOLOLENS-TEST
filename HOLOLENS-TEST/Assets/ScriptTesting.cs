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
    public JSONBaseCharacterPresetsWrapper basePresetsWrapper;

  
    void Start()
    {        
        basePresetsWrapper = JSONParser.ParseBaseCharacterPresets("JSONs/StatisticsExample");

        basePresets = FromJSONtoEngine.BaseCharacterPresetsTranslation(basePresetsWrapper.baseCharacterPresets);
        foreach(BaseCharacterPreset preset in basePresets.Values)
        {
            Debug.Log(preset.ToString());
        }

    }

}
