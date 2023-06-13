using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    Character character;
    public string charName;
    public string HP;
    // Start is called before the first frame update
    void Start()
    {
        character = GameManager.characterPool[charName];

        //Character registers itself in the GameManager list.
        GameManager.characterGameObjects.Add(gameObject);

        Debug.Log(character.ToString("  "));
    }

    // Update is called once per frame
    void Update()
    {
        HP = character.GetStats().GetStat("HP").GetCurrentValue().ToString();
    }

    public Character GetCharacter()
    {
        return character;
    }
}
