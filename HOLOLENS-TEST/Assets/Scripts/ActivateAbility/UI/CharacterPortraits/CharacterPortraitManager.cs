using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class CharacterPortraitManager : MonoBehaviour
{
    //set in Unity Editor
    public GameObject CharacterPortraitPrefab;

    public static CharacterPortraitManager Instance { get; private set; }

    List<GameObject> instances = new List<GameObject>();

    private void Awake()
    {
        // Ensure only one instance of GameManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManager objects
        }
    }

    public void Initialize()
    {
        string[] names = GameManager.GetInstance().turnManager.GetInitiativeNames();

        foreach(string name in names)
        {
            GameObject portrait = Instantiate(CharacterPortraitPrefab, transform);
            portrait.GetComponent<CharacterPortrait>().Initialize(name);
            
            PlacePortraitNextToLast(portrait);
            
            instances.Add(portrait);
        }
    }

    public void PlacePortraitNextToLast(GameObject portrait)
    {
        if (instances.Count == 0)
            return;

        float x = instances.Last().transform.localPosition.x + 300;
        float y = instances.Last().transform.localPosition.y;
        float z = instances.Last().transform.localPosition.z;

        portrait.transform.localPosition = new Vector3(x,y,z);
    }
}
