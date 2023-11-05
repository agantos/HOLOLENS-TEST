using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    Character character;
    public CharacterDialogue dialogue {get; private set; }

    public string charName;

    // Start is called before the first frame update
    void Start()
    {
        character = GameManager.GetInstance().characterPool[charName];

        //Character registers itself in the GameManager list.
        GameManager.GetInstance().playingCharacterGameObjects.Add(character.name, gameObject);
        GameManager.GetInstance().playingCharacterPool.Add(character.name, character);

        //Initialize Character Dialogue
        dialogue = new CharacterDialogue(charName);
        ScenarioSpecificMethods.GetInstance().InitializeCharacterDialog(dialogue);
    }

    void Update()
    {
        if (ScenarioSpecificMethods.GetInstance().ShouldDie(character))
        {
            Die();
        }
    }

    public Character GetCharacter()
    {
        return character;
    }

    void Die()
    {
        //Remove from initiative
        GameManager.GetInstance().KillCharacter(charName);

        //This character cannot be selected by abilities
        gameObject.GetComponent<Collider>().enabled = false;

        //Play Death Animation
        gameObject.GetComponent<AnimationManager>().IdleTo_Dying();
    }
}
