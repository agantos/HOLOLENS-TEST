using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    public Character stats;
    public string name;

    // Start is called before the first frame update
    void Start()
    {
        stats = new Character();
        stats.name = name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
