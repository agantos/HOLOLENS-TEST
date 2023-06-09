using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JSONBaseCharacterPreset
{
    public string name;
    public JSONstat[] statistics;
    public string[] abilities;
}

[System.Serializable]
public class JSONBaseCharacterPresetWrapper
{
    public JSONBaseCharacterPreset baseCharacterPreset;

    public JSONstat[] GetStatistics() { return baseCharacterPreset.statistics; }
    public string GetName() { return baseCharacterPreset.name; }
}

[System.Serializable]
public class JSONBaseCharacterPresetsWrapper
{
    public JSONBaseCharacterPreset[] baseCharacterPresets;
}
