using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ScriptTesting : MonoBehaviour
{
    //Init Singletons
    FromJSONtoEngine fromJSONtoEngineInstance = FromJSONtoEngine.GetInstance();
    JSONParser JSONParserInstance = JSONParser.GetInstance();

    //Variables
    public JSONBaseCharacterPresetWrapper basePresetWrapper;
    public CharacterStatistics stats;
    public BaseCharacterPreset human;
  
    void Start()
    {        
        basePresetWrapper = JSONParser.ParseBaseCharacterPreset("JSONs/StatisticsExample");
        human = FromJSONtoEngine.CreateBaseCharacterPreset(basePresetWrapper);
        human.stats.CalculateAllStats();
        Debug.Log(human.stats.ToString());
    }

}
