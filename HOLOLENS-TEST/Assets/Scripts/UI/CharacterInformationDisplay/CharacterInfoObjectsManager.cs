using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfoObjectsManager : MonoBehaviour
{
    //Set in unity editor
    public  GameObject CharacterDisplayInfoPrefab;
    public static CharacterInfoObjectsManager instance;

    static Dictionary<string, GameObject> instances = new Dictionary<string, GameObject>();
    static Dictionary<string, CharacterInfoDisplayManager> managers = new Dictionary<string, CharacterInfoDisplayManager>();

    void Start()
    {
        if(instance == null)
            instance = this;
    }

    public void CreateCharacterInfo(string characterName)
    {
        if (!managers.ContainsKey(characterName))
        {
            GameObject instance = Instantiate(CharacterDisplayInfoPrefab, transform);
            instances.Add(characterName, instance);
            managers.Add(characterName, instance.GetComponentInChildren<CharacterInfoDisplayManager>());
            
        }
        else
            managers[characterName].ClearUI();

        managers[characterName].CreateUI(characterName);
    }
}
