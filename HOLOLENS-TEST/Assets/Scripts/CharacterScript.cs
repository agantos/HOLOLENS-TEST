using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    Character character;
    public string charName;

    // Start is called before the first frame update
    void Start()
    {
        character = GameManager.GetInstance().characterPool[charName];

        //Character registers itself in the GameManager list.
        GameManager.GetInstance().playingCharacterGameObjects.Add(character.name, gameObject);
        GameManager.GetInstance().playingCharacterPool.Add(character.name, character);
    }

    public Character GetCharacter()
    {
        return character;
    }
}
