using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDisplayObjectsManager : MonoBehaviour
{
    //Set in unity editor
    public GameObject AbilityDisplayPrefab;
    public static AbilityDisplayObjectsManager instance;

    static Dictionary<string, GameObject> instances = new Dictionary<string, GameObject>();
    static Dictionary<string, AbilityDisplayManager> managers = new Dictionary<string, AbilityDisplayManager>();

    void Start()
    {
        if (instance == null)
            instance = this;
    }

    public void CreateAbilityDisplay(string abilityName)
    {
        if (!managers.ContainsKey(abilityName))
        {
            GameObject instance = Instantiate(AbilityDisplayPrefab, transform);
            instances.Add(abilityName, instance);
            managers.Add(abilityName, instance.GetComponent<AbilityDisplayManager>());

        }
        else
            managers[abilityName].ClearUI();

        managers[abilityName].CreateUI(abilityName);
    }

    public void DestroyAbilityDisplayInfo(string name)
    {
        instances.Remove(name);
        managers.Remove(name);
    }
}
