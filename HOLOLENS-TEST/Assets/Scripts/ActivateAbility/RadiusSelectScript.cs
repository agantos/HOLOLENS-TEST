using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusSelectScript : MonoBehaviour
{
    public GameObject selectToken;
    public int radius;
    //should represent what 1 feet is in the game
    public float baseRadiusX, baseRadiusY, baseRadiusZ;

    //Variables for the activation of the ability
    public Character attacker;
    List<Character> defenders = new List<Character>();
    public Ability ability;

    //- Adds collided Character to defenders
    //- Creates a token that indicates the collided is selected
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //Spawn selection token on top of player model.
            GameObject token = Object.Instantiate(selectToken);            
            token.transform.SetParent(other.transform.parent.transform, false);
            float playerHeight = other.transform.localPosition.y + other.transform.localScale.y / 2;
            token.transform.localPosition += new Vector3(0, playerHeight + 0.1f, 0);

            //Add character to the selected characters
            SelectTarget(other.gameObject.transform.parent.gameObject);
        }

    }

    //- Removes the select visual indication
    //- Removes collided from defenders
    void OnTriggerExit(Collider other)
    {
        //Destroy spawned selection token
        if(other.gameObject.tag == "Player")
        {
            Destroy(other.gameObject.transform.parent.transform.Find("IsSelected(Clone)").gameObject);
        }

        //Remove character from the selected characters
        UnselectTarget(other.gameObject.transform.parent.gameObject);
    }

    //Set the size the AOE selection
    public void SetScale()
    {
        gameObject.transform.localScale = new Vector3(radius * baseRadiusX, radius * baseRadiusY, radius * baseRadiusZ);
    }

    void SelectTarget(GameObject target) {
        defenders.Add(target.GetComponent<CharacterScript>().GetCharacter());
    }

    void SelectTarget(Collider other)
    {
        //Add character to the selected characters
        defenders.Add(other.gameObject.transform.parent.gameObject.GetComponent<CharacterScript>().GetCharacter());

        //Spawn selection token on top of player model.
        GameObject token = Object.Instantiate(selectToken);
        token.transform.SetParent(other.transform.parent.transform, false);
        float playerHeight = other.transform.localPosition.y + other.transform.localScale.y / 2;
        token.transform.localPosition += new Vector3(0, playerHeight + 0.1f, 0);
    }

    void UnselectTarget(GameObject target)
    {
        defenders.Remove(target.GetComponent<CharacterScript>().GetCharacter());
    }

    public void Activate()
    {
        foreach (Character defender in defenders)
        {
            AbilityManager.ActivateAbilityEffect(ability.name, defender, attacker);
        }
        Destroy(gameObject);
    }

}
