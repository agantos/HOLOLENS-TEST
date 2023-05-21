using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JSONstat
{
    public string name;
    public int staticValue;
    public string[] statRelations;
}

[System.Serializable]
public class JSONstats
{
    public JSONstat[] statistics;
}
