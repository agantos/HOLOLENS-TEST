using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class CharacterPortraitManager : MonoBehaviour
{
    //set in Unity Editor
    public GameObject CharacterPortraitPrefab;
    public GameObject CrystalPrefab;

    public static CharacterPortraitManager Instance { get; private set; }

    Dictionary<string, GameObject> instances = new Dictionary<string, GameObject>();
    GameObject crystalInstance;

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
        
        Transform prevPortraitTransform = null;

        //Create and place portraits
        foreach (string name in names)
        {             
            GameObject portrait = Instantiate(CharacterPortraitPrefab, transform);
            portrait.GetComponent<CharacterPortrait>().Initialize(name);
            
            PlacePortraitNextToLast(portrait, prevPortraitTransform);

            prevPortraitTransform = portrait.transform;
            
            instances.Add(name, portrait);
        }

        //Create and place Crystal
        crystalInstance = Instantiate(CrystalPrefab, transform);
        PlaceCrystal();
    }

    void PlacePortraitNextToLast(GameObject portrait, Transform last)
    {
        if (last == null)
            return;

        float x = last.localPosition.x + 300;
        float y = last.localPosition.y;
        float z = last.localPosition.z;

        portrait.transform.localPosition = new Vector3(x,y,z);
    }

    public void PlaceCrystal()
    {
        //Get Current Character position
        float x = instances[GameManager.GetInstance().GetCurrentPlayer_Name()].transform.localPosition.x;
        float y = 235;

        crystalInstance.transform.localPosition = new Vector3(x, y, 0);
    }
}
