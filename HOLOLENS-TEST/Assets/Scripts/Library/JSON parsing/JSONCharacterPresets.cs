using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JSONCharacterPreset
{
    public string name;
    public JSONstat[] statistics;
    public string[] abilities;
}

[System.Serializable]
public class JSONBaseCharacterPresetWrapper
{
    public JSONCharacterPreset baseCharacterPreset;

    public JSONstat[] GetStatistics() { return baseCharacterPreset.statistics; }
    public string GetName() { return baseCharacterPreset.name; }
}

[System.Serializable]
public class JSONBaseCharacterPresetsWrapper
{
    public JSONCharacterPreset[] baseCharacterPresets;
    public JSONCharacterPreset[] additionalCharacterPresets;
}
