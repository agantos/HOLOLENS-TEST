using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfoObjectsManager : MonoBehaviour
{
    //Set in unity editor
    public  GameObject CharacterDisplayInfoPrefab;
    public static CharacterInfoObjectsManager Instance { get; private set; }

    static Dictionary<string, GameObject> instances = new Dictionary<string, GameObject>();
    static Dictionary<string, CharacterInfoDisplayManager> managers = new Dictionary<string, CharacterInfoDisplayManager>();

    void Awake()
    {
        
    }

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManager objects
        }
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

    public void OnDestroyCharacterInformationObject(string name)
    {
        instances.Remove(name);
        managers.Remove(name);
    }
}
